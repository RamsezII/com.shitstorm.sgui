using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public class SguiCustom_Dropdown : SguiCustom_Abstract
    {
        public TMP_Dropdown _dropdown;
        [SerializeField] Toggle[] toggles;
        public readonly List<bool> currentValues_bool = new();
        public readonly HashSet<string> currentValues_string = new(StringComparer.Ordinal);
        float current_scrollheight = 1;
        [SerializeField] bool stay_open;
        public Action<SguiCustom_Dropdown> onValuesChanged;

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
                Scrollbar scrollbar = template_clone.GetComponentInChildren<Scrollbar>();

                scrollbar.value = 0;
                scrollbar.value = 1;
                scrollbar.value = current_scrollheight;
                scrollbar.onValueChanged.AddListener(value => current_scrollheight = value);

                toggles = template_clone.GetComponentsInChildren<Toggle>(true);

                currentValues_string.Clear();
                currentValues_bool.Clear();

                for (int i1 = 0; i1 < toggles.Length; i1++)
                {
                    Toggle item = toggles[i1];
                    string toggle_name = null;

                    if (i1 >= 3)
                    {
                        toggle_name = item.Get_ItemName_From_DropdownToggle();
                        currentValues_bool.Add(item.isOn);

                        if (item.isOn)
                            currentValues_string.Add(toggle_name);
                        else
                            currentValues_string.Remove(toggle_name);
                    }

                    int i2 = i1 - 3;
                    item.onValueChanged.AddListener(_ =>
                    {
                        current_scrollheight = scrollbar.value;
                        _dropdown.Show();

                        if (i2 >= 0)
                        {
                            currentValues_bool[i2] = item.isOn;
                            if (item.isOn)
                                currentValues_string.Add(toggle_name);
                            else
                                currentValues_string.Remove(toggle_name);
                        }

                        onValuesChanged?.Invoke(this);
                    });
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _dropdown.onValueChanged.RemoveAllListeners();
        }
    }
}