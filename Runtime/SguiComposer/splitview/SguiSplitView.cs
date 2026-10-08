using _ARK_;
using _UTIL_;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_.composer
{
    public sealed partial class SguiSplitView : ArkComponent1
    {
        [NonSerialized] public SguiComposer composer;
        [SerializeField] ScrollRect scrollview;
        [SerializeField] internal RectTransform rt_body, rt_dragzones;
        Dragzone[] dragzones;
        [SerializeField] TabHeader prefab_tabHeader;
        public readonly List<SguiFrame> frames = new();
        internal readonly ValueNotifier<TabHeader> current_tab = new();
        internal readonly ValueNotifier<TabHeader> current_drag = new();

        //--------------------------------------------------------------------------------------------------------------

        internal void Initialize()
        {
            composer = GetComponentInParent<SguiComposer>(includeInactive: true);

            dragzones = GetComponentsInChildren<Dragzone>(includeInactive: true);

            prefab_tabHeader.gameObject.SetActive(false);

            current_tab.AddListener(OnCurrentTab);

            current_drag.AddListener(() => rt_dragzones.gameObject.SetActive(current_drag.Has));
        }

        //--------------------------------------------------------------------------------------------------------------

        public SguiFrame AddTab(SguiFrame prefab)
        {
            composer.CancelCloseRequest();

            var header = Instantiate(prefab_tabHeader, parent: prefab_tabHeader.transform.parent);
            header.pview = this;
            header.gameObject.SetActive(true);
            header.Initialize();
            header.trad_title.SetTraductions(prefab.sgui_name);
            header.rimg_icon.texture = prefab.window_icon;

            if (header.trad_title.traductions.IsDefault)
                header.trad_title.SetText(prefab.GetType().FullName);

            bool prefabWasActive = prefab.gameObject.activeSelf;
            prefab.gameObject.SetActive(false);
            SguiFrame frame = Instantiate(prefab, rt_body);
            prefab.gameObject.SetActive(prefabWasActive);

            frame.pview = this;
            frame.tab = header;
            header.frame = frame;
            frame.transform.AsRTfm().FillParent();

            frame.Initialize();

            header.gameObject.SetActive(true);
            frame.gameObject.SetActive(true);

            frames.Add(frame);
            SelectFrame(frame);

            return frame;
        }

        int _lastSelectFrameCount;
        public void SelectFrame(SguiFrame frame)
        {
            if (Time.frameCount == _lastSelectFrameCount)
                if (frame == null)
                    SguiLoggerOverlay.Log($"SelectFrame null", this);
                else
                    SguiLoggerOverlay.Log($"SelectFrame {frame.sgui_name} ({frame.GetType().FullName})", frame);
            _lastSelectFrameCount = Time.frameCount;

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

        void OnCurrentTab(TabHeader selected)
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
            int indexOfFrame = frames.IndexOf(frame);
            if (indexOfFrame < 0)
                return;

            frames.RemoveAt(indexOfFrame);

            if (frame.tab != null)
                Destroy(frame.tab.gameObject);

            if (current_tab._value == frame.tab)
            {
                current_tab.Value = null;
                if (frames.Count > 0)
                    SelectFrame(frames[Mathf.Min(indexOfFrame, frames.Count - 1)]);
            }

            composer.OnFrameRemoved(frame);
        }

        internal void OnDropTab(in Dragzone dragzone, in TabHeader tab)
        {
            RemoveFrame(tab.frame);
            var rt = transform.AsRTfm();
            rt.anchorMin = dragzone.opposite_dragzone.rt_zone.anchorMin;
            rt.anchorMax = dragzone.opposite_dragzone.rt_zone.anchorMax;
            var newsplit = composer.AddSplitView(dragzone.rt_zone.anchorMin, dragzone.rt_zone.anchorMax);
            newsplit.AddTab(tab.frame);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDestroy()
        {
            current_tab.Clear();
            base.OnDestroy();
        }
    }
}
