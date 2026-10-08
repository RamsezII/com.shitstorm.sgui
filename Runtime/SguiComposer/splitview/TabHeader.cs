using _ARK_;
using _UTIL_;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_.composer
{
    internal sealed partial class TabHeader : ArkComponent1, SguiDragManager.IDraggable, SguiDragManager.IOnDraggedOver
    {
        internal SguiSplitView pview;
        [SerializeField] Button button;
        [SerializeField] Graphic graphic_selected;
        public SguiFrame frame;
        public Traductable trad_title;
        public readonly ValueNotifier<bool> isSelected = new();
        [SerializeField] internal RawImage rimg_icon;
        [SerializeField] TabInsertDrop insert_L, insert_R;

        //--------------------------------------------------------------------------------------------------------------

        internal void Initialize()
        {
            trad_title.onRefresh += RefreshSize;
            transform.Find("close").GetComponent<Button>().onClick.AddListener(() => frame.RequestClose());

            insert_L.Initialize();
            insert_R.Initialize();

            GetComponent<ContextListHandler>().callback += list =>
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

        void SguiDragManager.IDraggable.OnBegindDragExtra(PointerEventData eventData) => SguiSplitView.current_drag.Value = this;
        void SguiDragManager.IDraggable.OnEndDragExtra(PointerEventData eventData)
        {
            if (this == SguiSplitView.current_drag._value)
                SguiSplitView.current_drag.Value = null;
        }

        string SguiDragManager.IDraggable.DragDisplay => frame.sgui_name.GetAutomatic();
        object SguiDragManager.IDraggable.DragData => this;
        void SguiDragManager.IOnDraggedOver.OnDraggedOver(in SguiDragManager.IDraggable draggable, PointerEventData eventData) => frame.TakeFocus();

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDestroy()
        {
            if (this == SguiSplitView.current_drag._value)
                SguiSplitView.current_drag.Value = null;

            if (trad_title != null)
                trad_title.onRefresh -= RefreshSize;

            isSelected.Clear();

            base.OnDestroy();
        }
    }
}
