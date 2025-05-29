using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_
{
    internal class CompletorItem : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        SguiCompletor completor;
        RawImage background;
        public TextMeshProUGUI label;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            completor = GetComponentInParent<SguiCompletor>();
            background = GetComponent<RawImage>();
            label = transform.Find("label").GetComponent<TextMeshProUGUI>();
        }

        //--------------------------------------------------------------------------------------------------------------

        public void ToggleSelect(in bool value)
        {
            background.color = new(1, 1, 1, value ? .1f : 0);
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            completor.OnEnterItem(this);
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            completor.OnClickItem(this);
        }
    }
}