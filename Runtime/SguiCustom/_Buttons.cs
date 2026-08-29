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

        SguiCustom_Abstract AddNumber(in string label, in object value)
        {
            if (TryParseNumberField(label, value, out var slider, out var inputfield))
                return slider;
            else
                return inputfield;
        }

        SguiCustom_Dropdown_Normal AddEnum(in Enum value)
        {
            var dropdown = AddButton<SguiCustom_Dropdown_Normal>();

            Type type = value.GetType();
            var options = Enum.GetNames(type).Where(name => name switch
            {
                string n when n.StartsWith('_') && n.EndsWith('_') => false,
                _ => true,
            }).ToList();

            int selectedIndex = options.IndexOf(Enum.GetName(type, value));
            dropdown.SetupOptions(
                selectedIndex,
                options
            );

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
