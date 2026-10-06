using _ARK_;
using _SGUI_.context_click;
using _UTIL_;
using UnityEngine;
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

            listhandler.callback += list =>
            {
                list.AddButton_trad(new()
                {
                    french = "Fermer l’onglet",
                    english = "Close tab",
                })._button.onClick.AddListener(() => frame.RequestClose());

                list.AddButton_trad(new()
                {
                    french = "Fermer les autres onglets",
                    english = "Close other tabs",
                })._button.onClick.AddListener(() =>
                {
                    foreach (var frame in frame.pview.frames.ToArray())
                        if (this.frame != frame)
                            frame.RequestClose();
                });

                list.AddButton_trad(new()
                {
                    french = "Fermer tous les onglets",
                    english = "Close all tabs",
                })._button.onClick.AddListener(() =>
                {
                    foreach (var frame in frame.pview.frames.ToArray())
                        frame.RequestClose();
                });
            };

            isSelected.AddListener(graphic_selected.gameObject.SetActive);

            button.onClick.AddListener(() => frame.TakeFocus());
        }

        //--------------------------------------------------------------------------------------------------------------

        void RefreshSize()
        {
            var rt = (RectTransform)transform;
            rt.sizeDelta = new(Mathf.Max(110, trad_title.tmpro.preferredWidth), rt.sizeDelta.y);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDestroy()
        {
            if (trad_title != null)
                trad_title.onRefresh -= RefreshSize;

            isSelected.Clear();

            base.OnDestroy();
        }
    }
}
