using System;
using TMPro;
using UnityEngine;

namespace _SGUI_
{
    partial class SguiCustom
    {
        public bool TryParseNumberField(in string label, in object value, out SguiCustom_Slider slider, out SguiCustom_InputField inputfield)
        {
            bool is_int = value is int;
            bool is_float = value is float;
            bool is_number = is_int || is_float;

            slider = null;
            inputfield = null;

            if (label.StartsWith('_'))
                try
                {
                    string[] splits = label.Split('_', StringSplitOptions.None);
                    bool is_01 = splits[1].Equals("01", StringComparison.OrdinalIgnoreCase);

                    float min = is_01 ? 0 : splits[1].ToFloat();
                    float max = is_01 ? 1 : splits[2].ToFloat();

                    slider = AddButton<SguiCustom_Slider>();
                    slider.trad_label.SetTrad(splits[(is_01 ? 2 : 3)..].Join("_") + ":");
                    slider.slider.value = is_int ? (int)value : (float)value;
                    slider.slider.wholeNumbers = is_int;

                    slider.slider.minValue = min;
                    slider.slider.maxValue = max;

                    return true;
                }
                catch (Exception e)
                {
                    if (slider != null)
                        Destroy(slider.gameObject);
                    Debug.LogWarning(e.TrimmedMessage());
                }

            inputfield = AddButton<SguiCustom_InputField>();
            inputfield.trad_label.SetTrad(label + ":");
            inputfield.input_field.text = value.ToString();

            if (is_int)
                inputfield.input_field.contentType = TMP_InputField.ContentType.IntegerNumber;
            else if (is_float)
                inputfield.input_field.contentType = TMP_InputField.ContentType.DecimalNumber;

            return false;
        }
    }
}