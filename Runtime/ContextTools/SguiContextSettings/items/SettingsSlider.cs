using UnityEngine.UI;

namespace _SGUI_.context_tools.settings
{
    public sealed class SettingsSlider : ContextSetting_item
    {
        public TextLabel label;
        public Slider slider;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            label = GetComponentInChildren<TextLabel>(true);
            slider = GetComponentInChildren<Slider>(true);

            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();
            slider.onValueChanged.AddListener(value =>
            {
                label.text.text = System.Math.Round(value, 2).ToString().Replace(',', '.');
            });
            slider.value = 1;
        }
    }
}