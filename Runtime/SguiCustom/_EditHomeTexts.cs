using _ARK_;
using _UTIL_;
using System;
using System.Globalization;
using UnityEngine;

namespace _SGUI_
{
    partial class SguiCustom
    {
        public void EditArkText(IArkTexts target)
        {
            NUCLEOR.delegates.OnApplicationUnfocus += Oblivionize;
            onOblivion += () => NUCLEOR.delegates.OnApplicationUnfocus -= Oblivionize;

            foreach (var field in target.EFields<NJEditAttribute>())
            {
                object current_value = field.GetValue(target);
                SguiCustom_Abstract button = null;

                void SetValue<T>(T new_value)
                {
                    try
                    {
                        if (field.FieldType.IsEnum)
                            field.SetValue(target, Enum.ToObject(field.FieldType, new_value));
                        else
                            field.SetValue(target, Convert.ChangeType(new_value, field.FieldType, CultureInfo.InvariantCulture));
                        target.SaveArkText(log: true);
                        NUCLEOR.delegates.OnApplicationFocus?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning(ex.TrimmedExceptionMessage(), this);
                    }
                }

                switch (current_value)
                {
                    case bool _bool:
                        {
                            var toggle = AddBool(_bool);
                            button = toggle;
                            toggle.toggle.onValueChanged.AddListener(SetValue);
                        }
                        break;

                    case sbyte _sbyte:
                    case byte _byte:
                    case short _short:
                    case ushort _ushort:
                    case uint _uint:
                    case int _int:
                    case ulong _ulong:
                    case long _long:
                    case float _float:
                        {
                            var inpfield = AddNumber(name, current_value);
                            button = inpfield;
                            switch (inpfield)
                            {
                                case SguiCustom_InputField inputfield:
                                    inputfield.input_field.onSubmit.AddListener(SetValue);
                                    break;

                                case SguiCustom_Slider slider:
                                    slider.click_handler.onPointerUp += _ => SetValue(slider._slider.value);
                                    break;
                            }
                        }
                        break;

                    case Enum _enum:
                        {
                            var dd = AddEnum(_enum);
                            button = dd;
                            dd.onValueChanged += SetValue;
                        }
                        break;

                    case string _str:
                        {
                            var inpfield = AddString(_str);
                            button = inpfield;
                            inpfield.input_field.onSubmit.AddListener(SetValue);
                        }
                        break;

                    default:
                        {
                            var label = AddButton<SguiCustom_Label>();
                            button = label;
                            label.trad_label.SetTraductions(new()
                            {
                                english = $"Could not parse field: {{ \"{field.Name}\" : \"{current_value}\" }} ({field.FieldType})",
                                french = $"Impossible de parser le champ: {{ \"{field.Name}\" : \"{current_value}\" }} ({field.FieldType})",
                            });
                        }
                        break;
                }

                button.trad_label.SetText($"{field.Name}:");
            }
        }
    }
}
