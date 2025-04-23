using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    internal class HeaderDropdown : MonoBehaviour
    {
        public SguiWindow window;
        public Button button;
        public TMP_Dropdown dropdown;
        public Action<string> onItemClick;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            window = GetComponentInParent<SguiWindow>();
            RectTransform child_rt = (RectTransform)transform.GetChild(0);
            button = child_rt.GetComponent<Button>();
            dropdown = child_rt.GetComponent<TMP_Dropdown>();

            dropdown.transform.Find("template/mask/viewport/content/item").GetComponent<HeaderDropdownItem>().dropdown = this;
        }

        //--------------------------------------------------------------------------------------------------------------

        internal void OnItemClick(in HeaderDropdownItem item)
        {
            onItemClick?.Invoke(item.label.text);
        }
    }
}