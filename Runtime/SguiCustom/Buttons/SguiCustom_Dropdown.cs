using System;
using TMPro;

namespace _SGUI_
{
    public class SguiCustom_Dropdown : SguiCustom_Abstract
    {
        public readonly struct Output
        {
            public readonly int index;
            public readonly TMP_Dropdown.OptionData option_data;
            public override string ToString() => $"{index} {option_data.text.QuoteStringSafely()}";

            //--------------------------------------------------------------------------------------------------------------

            public Output(in int index, in TMP_Dropdown.OptionData option_data)
            {
                this.index = index;
                this.option_data = option_data;
            }
        }

        public TMP_Dropdown dropdown;
        public Action<SguiCustom_Dropdown_Template> on_template_clone;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            dropdown = transform.Find("dropdown").GetComponent<TMP_Dropdown>();
            dropdown.options.Clear();
            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        public void ToggleCheckmarks(in bool value)
        {
            dropdown.template.transform.Find("viewport/content/item/checkmark").gameObject.SetActive(true);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDispose()
        {
            base.OnDispose();
            dropdown.onValueChanged.RemoveAllListeners();
        }
    }
}