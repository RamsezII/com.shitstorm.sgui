using System;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_.context_tools.settings
{
    public sealed class SettingsToggle : ContextSetting_item
    {
        public Toggle toggle;
        public Action<bool> action;
        [SerializeField] int frame_flag;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            toggle = GetComponentInChildren<Toggle>(true);

            base.Awake();

            toggle.onValueChanged.AddListener(value =>
            {
                if (frame_flag < Time.frameCount)
                    action?.Invoke(value);
            });
        }

        //--------------------------------------------------------------------------------------------------------------

        public void SetValueNoCallback(bool value)
        {
            frame_flag = Time.frameCount;
            toggle.isOn = value;
        }
    }
}