using _ARK_;
using System;
using TMPro;

namespace _SGUI_
{
    public class OSHeaderDropdown : OSHeaderItem
    {
        public TMP_Dropdown dropdown;
        public Action<string> onItemClicked;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            dropdown = transform.Find("dropdown").GetComponent<TMP_Dropdown>();
            label = dropdown.transform.Find("label").GetComponent<Traductable>();
            base.Awake();
            dropdown.ClearOptions();
        }

        //--------------------------------------------------------------------------------------------------------------

        internal void OnItemClick(in string item)
        {
            onItemClicked(item);
        }
    }
}