using _ARK_;
using _SGUI_.context_click;
using _UTIL_;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_.composer
{
    internal sealed partial class SguiTabHeader : ArkComponent1
    {
        [SerializeField] ContextListHandler listhandler;
        [SerializeField] Button button;
        [SerializeField] Graphic graphic_selected;
        public SguiFrame frame;
        public Traductable trad_title;
        public readonly ValueNotifier<bool> isSelected = new();
        [SerializeField] internal RawImage rimg_icon;

        //--------------------------------------------------------------------------------------------------------------

        internal void Initialize()
        {
            trad_title.onRefresh += RefreshSize;
            transform.Find("close").GetComponent<Button>().onClick.AddListener(() => frame.RequestClose());

            if (listhandler != null)
                listhandler.callback += OnContextList;

            isSelected.AddListener(graphic_selected.gameObject.SetActive);

            button.onClick.AddListener(() => frame.TakeFocus());
        }

        //--------------------------------------------------------------------------------------------------------------

        void RefreshSize()
        {
            var rt = (RectTransform)transform;
            rt.sizeDelta = new(Mathf.Max(110, trad_title.tmpro.preferredWidth), rt.sizeDelta.y);
        }

        void OnContextList(ContextList list)
        {
            var close = list.AddButton_trad(new()
            {
                french = "Fermer l’onglet",
                english = "Close tab",
            });
            close._button.onClick.AddListener(() => frame.RequestClose());
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDestroy()
        {
            if (trad_title != null)
                trad_title.onRefresh -= RefreshSize;

            if (listhandler != null)
                listhandler.callback -= OnContextList;

            isSelected.Clear();

            base.OnDestroy();
        }
    }
}
