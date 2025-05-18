using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    public class OSHeaderDropdownItem : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] OSHeaderDropdown dropdown;
        [SerializeField] TextMeshProUGUI label;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            label = transform.Find("label").GetComponent<TextMeshProUGUI>();
            dropdown ??= GetComponentInParent<OSHeaderDropdown>();
        }

        //--------------------------------------------------------------------------------------------------------------

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            dropdown.OnItemClick(label.text);
        }
    }
}