using System;
using TMPro;
using UnityEngine.UI;

namespace _SGUI_
{
    public class SguiCustom_Dropdown : SguiCustom_Abstract
    {
        public TMP_Dropdown dropdown;
        public Action<SguiCustom_Dropdown_Template> on_template_clone;

        float current_scrollheight;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            dropdown = transform.Find("dropdown").GetComponent<TMP_Dropdown>();
            dropdown.options.Clear();
            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        public void ActivateMultiSelect()
        {
            dropdown.MultiSelect = true;
            dropdown.alphaFadeSpeed = 0;
            dropdown.template.transform.Find("viewport/content/item/checkmark").gameObject.SetActive(true);
        }

        internal void OnTemplateClone(SguiCustom_Dropdown_Template template_clone)
        {
            if (dropdown.MultiSelect)
            {
                Scrollbar scrollbar = template_clone.transform.Find("scrollbar").GetComponent<Scrollbar>();
                if (current_scrollheight != 0)
                    scrollbar.value = current_scrollheight;

                foreach (var item in template_clone.GetComponentsInChildren<Toggle>(true))
                    item.onValueChanged.AddListener(_ =>
                    {
                        dropdown.Show();
                        current_scrollheight = scrollbar.value;
                    });
            }
            on_template_clone?.Invoke(template_clone);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDispose()
        {
            base.OnDispose();
            dropdown.onValueChanged.RemoveAllListeners();
        }
    }
}