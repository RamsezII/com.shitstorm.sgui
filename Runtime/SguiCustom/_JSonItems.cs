using System.Reflection;
using System;
using TMPro;
using System.Linq;

namespace _SGUI_
{
    partial class SguiCustom
    {
        SguiCustom_Toggle AddBool(FieldInfo field, object target, in bool value, Action<SguiCustom_Abstract, object> on_change, ref Action on_save)
        {
            var toggle = AddButton<SguiCustom_Toggle>();
            toggle.trad_label.SetTrad(field.Name + ":");
            toggle.toggle.isOn = value;
            toggle.toggle.onValueChanged.AddListener(value => on_change(toggle, value));
            on_save += () => field.SetValue(target, toggle.toggle.isOn);
            return toggle;
        }

        SguiCustom_Abstract AddInt(FieldInfo field, object target, in int value, Action<SguiCustom_Abstract, object> on_change, ref Action on_save)
        {
            if (TryParseNumberField(field.Name, value, out var slider, out var inputfield))
            {
                slider.slider.onValueChanged.AddListener(value => on_change(slider, value));
                on_save += () => field.SetValue(target, (int)slider.slider.value);
                return slider;
            }
            else
            {
                inputfield.input_field.onValueChanged.AddListener(value => on_change(inputfield, value));
                on_save += () => field.SetValue(target, int.Parse(inputfield.input_field.text));
                return inputfield;
            }
        }

        SguiCustom_Abstract AddFloat(FieldInfo field, object target, in float value, Action<SguiCustom_Abstract, object> on_change, ref Action on_save)
        {
            if (TryParseNumberField(field.Name, value, out var slider, out var inputfield))
            {
                slider.slider.onValueChanged.AddListener(value => on_change(slider, value));
                on_save += () => field.SetValue(target, slider.slider.value);
                return slider;
            }
            else
            {
                inputfield.input_field.onValueChanged.AddListener(value => on_change(inputfield, value));
                on_save += () => field.SetValue(target, float.Parse(inputfield.input_field.text));
                return inputfield;
            }
        }

        SguiCustom_Dropdown AddEnum(FieldInfo field, object target, in Enum value, Action<SguiCustom_Abstract, object> on_change, ref Action on_save)
        {
            Type type = field.FieldType;

            var dropdown = AddButton<SguiCustom_Dropdown>();
            dropdown.trad_label.SetTrad(field.Name + ":");
            dropdown.dropdown.ClearOptions();
            dropdown.dropdown.AddOptions(Enum.GetNames(type).ToList());

            int enum_i = Convert.ToInt32(value);
            dropdown.dropdown.value = Enum.GetValues(type).Cast<int>().ToList().IndexOf(enum_i);
            dropdown.dropdown.RefreshShownValue();

            dropdown.dropdown.onValueChanged.AddListener(value => on_change(dropdown, value));

            on_save += () =>
            {
                int index = dropdown.dropdown.value;
                string enum_name = dropdown.dropdown.options[index].text;
                field.SetValue(target, Enum.Parse(type, enum_name));
            };

            return dropdown;
        }

        SguiCustom_InputField AddString(FieldInfo field, object target, in string value, Action<SguiCustom_Abstract, object> on_change, ref Action on_save)
        {
            var inputfield = AddButton<SguiCustom_InputField>();
            inputfield.trad_label.SetTrad(field.Name + ":");
            inputfield.input_field.text = value;
            inputfield.input_field.contentType = TMP_InputField.ContentType.Standard;
            inputfield.input_field.onValueChanged.AddListener(value => on_change(inputfield, value));
            on_save += () => field.SetValue(target, inputfield.input_field.text);
            return inputfield;
        }
    }
}