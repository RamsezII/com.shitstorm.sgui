using System;
using TMPro;
using System.Linq;
using System.Collections.Generic;

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

        SguiCustom_Abstract AddNumber(in string label, in object value)
        {
            if (TryParseNumberField(label, value, out var slider, out var inputfield))
                return slider;
            else
                return inputfield;
        }

        SguiCustom_Dropdown AddEnum(in Enum value)
        {
            var dropdown = AddButton<SguiCustom_Dropdown>();

            Type type = value.GetType();
            Array values = Enum.GetValues(type);
            string[] names = Enum.GetNames(type);

            List<string> options = names.Where(name => name switch
            {
                string n when n.StartsWith('_') && n.EndsWith('_') => false,
                _ => true,
            }).ToList();

            dropdown._dropdown.ClearOptions();
            dropdown._dropdown.AddOptions(options);
            dropdown._dropdown.value = Array.IndexOf(values, value);
            dropdown._dropdown.RefreshShownValue();

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