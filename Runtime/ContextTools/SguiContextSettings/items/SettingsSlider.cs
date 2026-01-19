using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_.context_tools.settings
{
    public sealed class SettingsSlider : ContextSetting_item
    {
        public TextInput input;
        public Slider slider;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            input = GetComponentInChildren<TextInput>(true);
            slider = GetComponentInChildren<Slider>(true);

            base.Awake();

            slider.onValueChanged.AddListener(SetInputValue);

            input.onSubmit += text =>
            {
                if (Util.TryParseFloat(text, out float value))
                {
                    value = Mathf.Clamp(value, slider.minValue, slider.maxValue);
                    slider.value = value;
                }
                SetInputValue(slider.value);
            };
        }

        //--------------------------------------------------------------------------------------------------------------

        void SetInputValue(float value)
        {
            input.SetValueNoSubmit(System.Math.Round(value, 2).ToString().Replace(',', '.'));
        }
    }
}