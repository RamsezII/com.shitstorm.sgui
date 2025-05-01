using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public class SguiCustomButton_Slider : SguiCustomButton
    {
        public Slider slider;
        [SerializeField] TextMeshProUGUI tmp_value;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            slider = transform.Find("slider").GetComponent<Slider>();
            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();
            slider.onValueChanged.AddListener(OnSliderValue);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnSliderValue(float value)
        {
            tmp_value.text = Math.Round(value, 1).ToString();
        }
    }
}