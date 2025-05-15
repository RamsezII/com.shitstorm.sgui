using System;
using TMPro;
using System.Linq;

namespace _SGUI_
{
    partial class SguiCustom
    {
        SguiCustom_Toggle AddBool(in bool value)
        {
            var toggle = AddButton<SguiCustom_Toggle>();
            toggle.toggle.isOn = value;
            return toggle;
        }

        SguiCustom_Abstract AddInt(in string label, in int value)
        {
            if (TryParseNumberField(label, value, out var slider, out var inputfield))
                return slider;
            else
                return inputfield;
        }

        SguiCustom_Abstract AddFloat(in string label, in float value)
        {
            if (TryParseNumberField(label, value, out var slider, out var inputfield))
                return slider;
            else
                return inputfield;
        }

        SguiCustom_Dropdown AddEnum(in Enum value)
        {
            var dropdown = AddButton<SguiCustom_Dropdown>();
            dropdown.dropdown.ClearOptions();
            dropdown.dropdown.AddOptions(Enum.GetNames(value.GetType()).ToList());

            int enum_i = Convert.ToInt32(value);
            dropdown.dropdown.value = Enum.GetValues(value.GetType()).Cast<int>().ToList().IndexOf(enum_i);
            dropdown.dropdown.RefreshShownValue();

            return dropdown;
        }

        SguiCustom_InputField AddString(in string value)
        {
            var inputfield = AddButton<SguiCustom_InputField>();
            inputfield.input_field.text = value;
            inputfield.input_field.contentType = TMP_InputField.ContentType.Standard;
            return inputfield;
        }
    }
}