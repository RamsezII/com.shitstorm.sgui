using _ARK_;
using _UTIL_;
using System;
using System.Collections.Generic;
using Unity.Scripting.LifecycleManagement;
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
        [AutoStaticsCleanup] internal static readonly ValueNotifier<TabHeader> current_drag = new();

        //--------------------------------------------------------------------------------------------------------------

        internal void Initialize()
        {
            composer = GetComponentInParent<SguiComposer>(includeInactive: true);

            dragzones = GetComponentsInChildren<Dragzone>(includeInactive: true);

            prefab_tabHeader.gameObject.SetActive(false);

            current_tab.AddListener(OnCurrentTab);

            current_drag.AddListener(OnCurrentDrag);
        }

        void OnCurrentDrag() => rt_dragzones.gameObject.SetActive(current_drag.Has && (current_drag._value.pview != this || frames.Count > 1));

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

        internal void RemoveFrame(SguiFrame frame, bool destroyTab = true)
        {
            int indexOfFrame = frames.IndexOf(frame);
            if (indexOfFrame < 0)
                return;

            frames.RemoveAt(indexOfFrame);

            if (destroyTab && frame.tab != null)
                Destroy(frame.tab.gameObject);

            if (current_tab._value == frame.tab)
            {
                current_tab.Value = null;
                if (frames.Count > 0)
                    SelectFrame(frames[Mathf.Min(indexOfFrame, frames.Count - 1)]);
            }

            if (frames.Count == 0)
                composer.RemoveSplitView(this);

            composer.OnFrameRemoved(frame);
        }

        internal void OnDropTab(in Dragzone dragzone, in TabHeader tab)
        {
            var source = tab.pview;
            var frame = tab.frame;
            if (source == this && frames.Count == 1)
                return;

            var newsplit = composer.SplitView(this, dragzone);

            current_drag.Value = null;
            composer.CancelCloseRequest();
            source.composer.CancelCloseRequest();
            frame.isFocused.Value = false;
            frame.pview = tab.pview = newsplit;
            tab.transform.SetParent(newsplit.prefab_tabHeader.transform.parent, false);
            frame.transform.SetParent(newsplit.rt_body, false);
            frame.transform.AsRTfm().FillParent();
            frame.canvas = frame.GetComponentInParent<Canvas>(true);
            frame.raycaster = frame.GetComponentInParent<GraphicRaycaster>(true);
            newsplit.frames.Add(frame);
            source.RemoveFrame(frame, destroyTab: false);
            frame.TakeFocus();
            composer.OnResized();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDestroy()
        {
            current_drag.RemoveListener(OnCurrentDrag);
            if (current_drag._value != null && current_drag._value.pview == this)
                current_drag.Value = null;
            current_tab.Clear();
            base.OnDestroy();
        }
    }
}
