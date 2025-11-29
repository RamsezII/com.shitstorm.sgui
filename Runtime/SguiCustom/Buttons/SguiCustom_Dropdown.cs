using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public class SguiCustom_Dropdown : SguiCustom_Abstract
    {
        public TMP_Dropdown _dropdown;
        public Action<SguiCustom_Dropdown_Template> on_template_clone;

        public Dictionary<string, bool> toggles;
        float current_scrollheight = 1;
        [SerializeField] bool stay_open;

        public IEnumerable<string> ESelectedItems() => toggles?.Where(pair => pair.Value).Select(pair => pair.Key);

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            _dropdown = transform.Find("dropdown").GetComponent<TMP_Dropdown>();
            _dropdown.options.Clear();
            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        public void ToggleCheckmarks(in bool toggle) => _dropdown.template.transform.Find("viewport/content/item/checkmark").gameObject.SetActive(toggle);

        public void StayOpen()
        {
            stay_open = true;
            _dropdown.alphaFadeSpeed = 0;
        }

        public void ActivateMultiSelect()
        {
            StayOpen();
            _dropdown.MultiSelect = true;
            ToggleCheckmarks(true);
        }

        internal void OnTemplateClone(SguiCustom_Dropdown_Template template_clone)
        {
            if (stay_open)
            {
                Scrollbar scrollbar = template_clone.transform.Find("scrollbar").GetComponent<Scrollbar>();

                scrollbar.value = 0;
                scrollbar.value = 1;
                scrollbar.value = current_scrollheight;
                scrollbar.onValueChanged.AddListener(value => current_scrollheight = value);

                toggles?.Clear();
                toggles = new(StringComparer.OrdinalIgnoreCase);

                Toggle[] items = template_clone.GetComponentsInChildren<Toggle>(true);
                for (int i = 0; i < items.Length; i++)
                {
                    Toggle item = items[i];
                    string toggle_name = null;

                    if (i > 2)
                    {
                        toggle_name = item.Get_ItemName_From_DropdownToggle();
                        toggles.Add(toggle_name, item.isOn);
                    }

                    int i_copy = i;
                    item.onValueChanged.AddListener(_ =>
                    {
                        if (i_copy > 2)
                            toggles[toggle_name] = item.isOn;
                        current_scrollheight = scrollbar.value;
                        _dropdown.Show();
                    });
                }
            }
            on_template_clone?.Invoke(template_clone);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDispose()
        {
            base.OnDispose();
            _dropdown.onValueChanged.RemoveAllListeners();
        }
    }
}