using System.Reflection;
using System;
using _ARK_;

namespace _SGUI_
{
    partial class SguiCustom
    {
        public void ReflectionEditor<T>(T target, Action<object> on_confirm, Traductions title = default)
        {
            object result = target;

            if (title.IsDefault)
                title = new(result.GetType().FullName);

            trad_title.SetTrads(title);
            void OnChange() => trad_title.SetTrad(title + "*");

            FieldInfo[] target_fields = result.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            PropertyInfo[] target_properties = result.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            for (int i = 0; i < target_fields.Length; ++i)
            {
                FieldInfo field = target_fields[i];

                string field_name = field.Name;
                Type field_type = field.FieldType;
                object field_value = field.GetValue(result);

                if (TryGenerateButton(field_name, field_value, out SguiCustom_Abstract button))
                {
                    button.trad_label.SetTrads(new() { french = field_name + " :", english = field_name + ":", });
                    switch (button)
                    {
                        case SguiCustom_Toggle toggle:
                            onAction_confirm += () => field.SetValue(result, toggle.toggle.isOn);
                            break;

                        case SguiCustom_InputField inputfield:
                            switch (field_value)
                            {
                                case int _int:
                                    onAction_confirm += () => field.SetValue(result, Convert.ToInt32(inputfield.input_field.text));
                                    break;
                                case float _float:
                                    onAction_confirm += () => field.SetValue(result, Util.ParseFloat(inputfield.input_field.text));
                                    break;
                                default:
                                    onAction_confirm += () => field.SetValue(result, inputfield.input_field.text);
                                    break;
                            }
                            break;

                        case SguiCustom_Slider slider:
                            switch (field_value)
                            {
                                case int _int:
                                    onAction_confirm += () => field.SetValue(result, Convert.ToInt32(slider.slider.value));
                                    break;
                                case float _float:
                                    onAction_confirm += () => field.SetValue(result, slider.slider.value);
                                    break;
                            }
                            break;

                        case SguiCustom_Dropdown dropdown:
                            onAction_confirm += () =>
                            {
                                int index = dropdown.dropdown.value;
                                string enum_name = dropdown.dropdown.GetSelectedName();
                                field.SetValue(result, Enum.Parse(field_type, enum_name));
                                ;
                            };
                            break;
                    }
                }
                else
                {
                    button = AddButton<SguiCustom_Label>();
                    button.trad_label.SetTrads(new()
                    {
                        english = $"Could not parse field: {{ \"{field_name}\" : \"{field_value}\" }} ({field_type})",
                        french = $"Impossible de parser le champ: {{ \"{field_name}\" : \"{field_value}\" }} ({field_type})",
                    });
                }
            }

            for (int i = 0; i < target_properties.Length; ++i)
            {
                PropertyInfo property = target_properties[i];
                string property_name = property.Name;
                Type property_type = property.PropertyType;
                object property_value = property.GetValue(result);

                if (TryGenerateButton(property_name, property_value, out SguiCustom_Abstract button))
                {
                    button.trad_label.SetTrads(new() { french = property_name + " :", english = property_name + ":", });
                    switch (button)
                    {
                        case SguiCustom_Toggle toggle:
                            onAction_confirm += () => result = result.ModifyAnonymous(property_name, toggle.toggle.isOn);
                            break;

                        case SguiCustom_InputField inputfield:
                            switch (property_value)
                            {
                                case int _int:
                                    onAction_confirm += () => result = result.ModifyAnonymous(property_name, Convert.ToInt32(inputfield.input_field.text));
                                    break;
                                case float _float:
                                    onAction_confirm += () => result = result.ModifyAnonymous(property_name, Util.ParseFloat(inputfield.input_field.text));
                                    break;
                                default:
                                    onAction_confirm += () => result = result.ModifyAnonymous(property_name, inputfield.input_field.text);
                                    break;
                            }
                            break;

                        case SguiCustom_Slider slider:
                            switch (property_value)
                            {
                                case int _int:
                                    onAction_confirm += () => result = result.ModifyAnonymous(property_name, Convert.ToInt32(slider.slider.value));
                                    break;
                                case float _float:
                                    onAction_confirm += () => result = result.ModifyAnonymous(property_name, slider.slider.value);
                                    break;
                            }
                            break;

                        case SguiCustom_Dropdown dropdown:
                            onAction_confirm += () =>
                            {
                                int index = dropdown.dropdown.value;
                                string enum_name = dropdown.dropdown.GetSelectedName();
                                result = result.ModifyAnonymous(property_name, Enum.Parse(property_type, enum_name));
                            };
                            break;
                    }
                }
                else
                {
                    button = AddButton<SguiCustom_Label>();
                    button.trad_label.SetTrads(new()
                    {
                        english = $"Could not parse field: {{ \"{property_name}\" : \"{property_value}\" }} ({property_type})",
                        french = $"Impossible de parser le champ: {{ \"{property_name}\" : \"{property_value}\" }} ({property_type})",
                    });
                }
            }

            onAction_confirm += () => on_confirm?.Invoke(result);

            bool TryGenerateButton(in string name, in object value, out SguiCustom_Abstract button)
            {
                switch (value)
                {
                    case bool _bool:
                        {
                            SguiCustom_Toggle toggle = AddBool(_bool);
                            button = toggle;
                            toggle.toggle.onValueChanged.AddListener(value => OnChange());
                        }
                        break;

                    case int _int:
                        {
                            button = AddInt(name, _int);

                            switch (button)
                            {
                                case SguiCustom_InputField inputfield:
                                    inputfield.input_field.onValueChanged.AddListener(value => OnChange());
                                    break;

                                case SguiCustom_Slider slider:
                                    slider.slider.onValueChanged.AddListener(value => OnChange());
                                    break;
                            }
                        }
                        break;

                    case float _float:
                        {
                            button = AddFloat(name, _float);

                            switch (button)
                            {
                                case SguiCustom_InputField inputfield:
                                    inputfield.input_field.onValueChanged.AddListener(value => OnChange());
                                    break;

                                case SguiCustom_Slider slider:
                                    slider.slider.onValueChanged.AddListener(value => OnChange());
                                    break;
                            }
                        }
                        break;

                    case Enum _enum:
                        {
                            SguiCustom_Dropdown dropdown = AddEnum(_enum);
                            button = dropdown;
                            dropdown.dropdown.onValueChanged.AddListener(value => OnChange());
                        }
                        break;

                    case string _str:
                        {
                            SguiCustom_InputField inputfield = AddString(_str);
                            button = inputfield;
                            inputfield.input_field.onValueChanged.AddListener(value => OnChange());
                        }
                        break;

                    default:
                        button = null;
                        break;
                }

                return button != null;
            }
        }
    }
}