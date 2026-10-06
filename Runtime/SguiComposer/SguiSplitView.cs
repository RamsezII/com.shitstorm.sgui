using _ARK_;
using _UTIL_;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_.composer
{
    public sealed partial class SguiSplitView : ArkComponent1
    {
        internal readonly ValueNotifier<SguiTabHeader> current_tab = new();
        [SerializeField] internal RectTransform rt_body;
        public SguiComposer composer;
        [SerializeField] ScrollRect scrollview;
        [SerializeField] SguiTabHeader prefab_tabHeader;
        public readonly List<SguiFrame> frames = new();

        //--------------------------------------------------------------------------------------------------------------

        internal void Initialize()
        {
            composer = GetComponentInParent<SguiComposer>(true);

            prefab_tabHeader.gameObject.SetActive(false);

            current_tab.AddListener(OnCurrentTab);
        }

        //--------------------------------------------------------------------------------------------------------------

        public SguiFrame AddTab(SguiFrame prefab)
        {
            composer.CancelCloseRequest();

            var header = Instantiate(prefab_tabHeader, parent: prefab_tabHeader.transform.parent);
            header.gameObject.SetActive(true);
            header.Initialize();
            header.trad_title.SetTraductions(prefab.sgui_name);

            if (header.trad_title.traductions.IsDefault)
                header.trad_title.SetText(prefab.GetType().FullName);

            // Clone an inactive frame without requiring an inactive prefab asset.
            bool prefabWasActive = prefab.gameObject.activeSelf;
            SguiFrame frame;
            prefab.gameObject.SetActive(false);
            try
            {
                frame = Instantiate(prefab, rt_body);
            }
            finally
            {
                prefab.gameObject.SetActive(prefabWasActive);
            }
            frame.pview = this;
            frame.tab = header;
            header.frame = frame;

            frame.Initialize();

            header.rimg_icon.texture = prefab.window_icon;
            frame.transform.AsRTfm().FillParent();

            frames.Add(frame);
            header.gameObject.SetActive(true);
            SelectFrame(frame);

            return frame;
        }

        public void SelectFrame(SguiFrame frame)
        {
            SguiLoggerOverlay.Log($"SelectFrame {frame?.sgui_name} ({frame?.GetType().FullName})");

            if (frame == null || frame.oblivionized || !frames.Contains(frame))
                return;

            current_tab.Value = frame.tab;
            composer.active_frame.Value = frame;

            Canvas.ForceUpdateCanvases();

            var bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(scrollview.viewport, frame.tab.transform);
            var viewport = scrollview.viewport.rect;
            float correction = bounds.min.x < viewport.xMin ? viewport.xMin - bounds.min.x : bounds.max.x > viewport.xMax ? viewport.xMax - bounds.max.x : 0;
            scrollview.StopMovement();
            scrollview.content.anchoredPosition += new Vector2(correction, 0);
        }

        void OnCurrentTab(SguiTabHeader selected)
        {
            foreach (var frame in frames)
                if (frame != null && !frame.oblivionized)
                {
                    bool active = frame.tab == selected;
                    frame.tab.isSelected.Value = active;

                    if (!active)
                        frame.isFocused.Value = false;

                    frame.gameObject.SetActive(active);

                    if (active)
                        frame.OnResized();
                }
        }

        internal void RemoveFrame(SguiFrame frame)
        {
            int index = frames.IndexOf(frame);
            if (index < 0) return;
            frames.RemoveAt(index);
            if (frame.tab != null) Destroy(frame.tab.gameObject);
            bool selected = current_tab._value == frame.tab;
            if (selected) current_tab.Value = null;
            if (selected && frames.Count > 0) SelectFrame(frames[Mathf.Min(index, frames.Count - 1)]);
            composer.OnFrameRemoved(frame);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDestroy()
        {
            current_tab.Clear();
            base.OnDestroy();
        }
    }
}
