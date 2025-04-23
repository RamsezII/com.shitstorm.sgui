using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    internal sealed class HeaderDropdownItem : MonoBehaviour, IPointerClickHandler
    {
        public HeaderDropdown dropdown;

        //--------------------------------------------------------------------------------------------------------------

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            dropdown.OnItemClick(this);
        }
    }
}