using System;
using TMPro;
using UnityEngine;

namespace _SGUI_.context_tools.settings
{
    public class SettingsInputfield : ContextSetting_item
    {
        [SerializeField] protected TMP_InputField inputfield;
        [SerializeField] TextMeshProUGUI lint;

        internal Func<string, string> onValueChanged;
        public Action<string> onSubmit;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            inputfield = GetComponentInChildren<TMP_InputField>(true);
            lint = inputfield.transform.Find("text-area/lint").GetComponent<TextMeshProUGUI>();

            inputfield.onValueChanged.AddListener(text => lint.text = onValueChanged?.Invoke(text) ?? text);
            inputfield.onSubmit.AddListener(text => onSubmit?.Invoke(text));

            base.Awake();
        }
    }
}