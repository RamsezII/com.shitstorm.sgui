using System;
using TMPro;
using UnityEngine;

namespace _SGUI_
{
    partial class SguiCustom
    {
        public bool TryParseNumberField(in string label, in object value, out SguiCustom_Slider slider, out SguiCustom_InputField inputfield)
        {
            bool is_wholeNumber = value switch
            {
                sbyte or byte or short or ushort or int or uint or long or ulong => true,
                _ => false,
            };

            bool is_float = value is float;
            bool is_number = is_wholeNumber || is_float;

            slider = null;
            inputfield = null;

            if (label.StartsWith('_'))
                try
                {
                    string[] splits = label.Split('_', StringSplitOptions.None);
                    bool is_01 = splits[1].Equals("01", StringComparison.OrdinalIgnoreCase);

                    float min = is_01 ? 0 : splits[1].ParseFloat();
                    float max = is_01 ? 1 : splits[2].ParseFloat();

                    slider = AddButton<SguiCustom_Slider>();
                    slider.trad_label.SetTrad(splits[(is_01 ? 2 : 3)..].Join("_") + ":");

                    slider.slider.wholeNumbers = is_wholeNumber;
                    slider.slider.minValue = min;
                    slider.slider.maxValue = max;

                    if (is_wholeNumber)
                        slider.slider.value = value switch
                        {
                            sbyte _sbyte => _sbyte,
                            byte _byte => _byte,
                            short _short => _short,
                            ushort _ushort => _ushort,
                            int _int => _int,
                            uint _uint => _uint,
                            long _long => _long,
                            ulong _ulong => _ulong,
                            _ => throw new NotImplementedException($"wrong type for value \"{value}\" ({value.GetType()})")
                        };
                    else
                        slider.slider.value = (float)value;

                    return true;
                }
                catch (Exception e)
                {
                    if (slider != null)
                        Destroy(slider.gameObject);
                    Debug.LogWarning(e.TrimmedExceptionMessage());
                }

            inputfield = AddButton<SguiCustom_InputField>();
            inputfield.trad_label.SetTrad(label + ":");
            inputfield.input_field.text = value.ToString().Replace(',', '.');

            if (is_wholeNumber)
                inputfield.input_field.contentType = TMP_InputField.ContentType.IntegerNumber;
            else if (is_float)
            {
                inputfield.input_field.onValidateInput += (text, charIndex, addedChar) => addedChar switch
                {
                    ',' or '.' when !text.Contains('.', StringComparison.OrdinalIgnoreCase) => '.',
                    char c when c >= '0' && c <= '9' => addedChar,
                    _ => '\0',
                };
                inputfield.input_field.contentType = TMP_InputField.ContentType.DecimalNumber;
            }

            return false;
        }
    }
}