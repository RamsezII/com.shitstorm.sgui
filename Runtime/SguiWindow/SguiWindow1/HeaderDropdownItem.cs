using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    internal sealed class HeaderDropdownItem : MonoBehaviour, IPointerClickHandler
    {
        public HeaderDropdown dropdown;
        [HideInInspector] public TextMeshProUGUI label;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            label = transform.Find("label").GetComponent<TextMeshProUGUI>();
        }

        //--------------------------------------------------------------------------------------------------------------

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            dropdown.OnItemClick(this);
        }
    }
}