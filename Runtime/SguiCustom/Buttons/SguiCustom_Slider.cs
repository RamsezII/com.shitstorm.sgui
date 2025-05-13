using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public class SguiCustom_Slider : SguiCustom_Abstract
    {
        public Slider slider;
        [SerializeField] TextMeshProUGUI tmp_value;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            slider = transform.Find("slider").GetComponent<Slider>();
            tmp_value = transform.Find("value").GetComponent<TextMeshProUGUI>();
            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();
            slider.onValueChanged.AddListener(OnSliderValue);
            OnSliderValue(slider.value);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnSliderValue(float value)
        {
            tmp_value.text = value.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDispose()
        {
            base.OnDispose();
            slider.onValueChanged.RemoveAllListeners();
        }
    }
}