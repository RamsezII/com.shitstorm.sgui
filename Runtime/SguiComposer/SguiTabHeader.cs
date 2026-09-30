using _ARK_;
using _SGUI_.context_click;
using _UTIL_;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_.composer
{
    internal sealed partial class SguiTabHeader : ArkComponent1, IPointerClickHandler, SguiGlobal.ISguiGlobalLeftClick
    {
        [SerializeField] ContextListHandler listhandler;
        public SguiFrame frame;
        public Traductable trad_title;
        public readonly ValueNotifier<bool> isSelected = new();
        [SerializeField] RawImage background;

        protected override void Awake()
        {
            base.Awake();

            trad_title.onRefresh += RefreshSize;
            isSelected.AddListener(selected => background.color = new Color(1, 1, 1, selected ? .3f : .1f));
            transform.Find("close").GetComponent<Button>().onClick.AddListener(() => frame.RequestClose());

            if (listhandler != null)
                listhandler.callback += OnContextList;
        }

        internal void SetIcon(Texture texture)
        {
            var image = transform.Find("software-icon")?.GetComponent<RawImage>();
            if (image != null) image.texture = texture;
        }

        void RefreshSize()
        {
            var rt = (RectTransform)transform;
            rt.sizeDelta = new(Mathf.Max(110, trad_title.tmpro.preferredWidth), rt.sizeDelta.y);
        }

        public void OnSguiGlobalLeftClick() => frame.TakeFocus();
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left) frame?.TakeFocus();
            else if (eventData.button == PointerEventData.InputButton.Middle) frame?.RequestClose();
        }

        void OnContextList(ContextList list)
        {
            var close = list.AddButton_trad(new() { french = "Fermer l’onglet", english = "Close tab" });
            close._button.onClick.AddListener(() => frame?.RequestClose());
        }

        protected override void OnDestroy()
        {
            if (trad_title != null) trad_title.onRefresh -= RefreshSize;
            if (listhandler != null) listhandler.callback -= OnContextList;
            isSelected.Clear();
            base.OnDestroy();
        }
    }
}
