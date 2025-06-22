using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    public sealed class ShellField : TMP_InputField
    {
        ShellView shellview;
        public RectTransform rT;
        public TextMeshProUGUI lint;

        //----------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            shellview = GetComponentInParent<ShellView>();
            rT = (RectTransform)transform;
            lint = transform.Find("area/lint").GetComponent<TextMeshProUGUI>();

            base.Awake();
        }

        //----------------------------------------------------------------------------------------------------------

        public override void OnScroll(PointerEventData eventData)
        {
            base.OnScroll(eventData);
            shellview.scrollview.OnScroll(eventData);
        }
    }
}