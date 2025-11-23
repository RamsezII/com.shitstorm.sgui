using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_
{
    public sealed class ShellField : TMP_InputField
    {
        ShellView shellview;
        ScrollRect scrollview;
        public RectTransform rT;
        public TextMeshProUGUI lint;

        //----------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            scrollview = GetComponentInParent<ScrollRect>();
            shellview = GetComponentInParent<ShellView>();
            rT = (RectTransform)transform;
            lint = transform.Find("area/lint").GetComponent<TextMeshProUGUI>();

            base.Awake();
        }

        //----------------------------------------------------------------------------------------------------------

        public override void OnScroll(PointerEventData eventData)
        {
            base.OnScroll(eventData);
            scrollview.OnScroll(eventData);
        }
    }
}