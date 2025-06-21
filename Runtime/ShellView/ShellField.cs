using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    public sealed class ShellField : MonoBehaviour, IScrollHandler
    {
        ShellView shellview;
        public RectTransform rT;
        public TMP_InputField inputfield;
        public TextMeshProUGUI lint;

        //----------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            shellview = GetComponentInParent<ShellView>();
            rT = (RectTransform)transform;
            inputfield = GetComponent<TMP_InputField>();
            lint = transform.Find("area/lint").GetComponent<TextMeshProUGUI>();
        }

        //----------------------------------------------------------------------------------------------------------

        void IScrollHandler.OnScroll(PointerEventData eventData)
        {
            shellview.scrollview.OnScroll(eventData);
        }
    }
}