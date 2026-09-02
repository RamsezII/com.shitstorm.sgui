using _UTIL_;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public class SguiCustom_Slider : SguiCustom_Abstract
    {
        public Slider _slider;
        public PointerClickHandler click_handler;
        [SerializeField] TextMeshProUGUI tmp_value;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            _slider = transform.Find("slider").GetComponent<Slider>();
            click_handler = _slider.GetComponent<PointerClickHandler>();
            tmp_value = transform.Find("value").GetComponent<TextMeshProUGUI>();
            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();
            _slider.onValueChanged.AddListener(OnSliderValue);
            OnSliderValue(_slider.value);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnSliderValue(float value)
        {
            tmp_value.text = value.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _slider.onValueChanged.RemoveAllListeners();
        }
    }
}