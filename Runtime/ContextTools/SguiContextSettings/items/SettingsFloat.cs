using _UTIL_;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_.context_tools.settings
{
    public sealed class SettingsFloat : SettingsInputfield
    {
        public Action<float> onSubmitFloat;
        public float current_float;
        public float drag_speed = 1;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            base.Awake();

            onValueChanged += text => Util.TryParseFloat(text, out float value) ? text : text.SetColor(Color.red);

            inputfield.onSubmit.AddListener(text =>
            {
                if (Util.TryParseFloat(text, out float value))
                {
                    current_float = value;
                    onSubmitFloat?.Invoke(value);
                }
                inputfield.text = Util.FloatToString(current_float);
            });
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            inputfield.text = Util.FloatToString(current_float);

            DragHandler drag_handler = label_trad.tmpro.transform.GetComponent<DragHandler>();
            drag_handler.onDrag += (PointerEventData eventData) =>
            {
                current_float += drag_speed * eventData.delta.x;
                inputfield.text = Util.FloatToString(current_float);
                onSubmitFloat?.Invoke(current_float);
            };
        }
    }
}