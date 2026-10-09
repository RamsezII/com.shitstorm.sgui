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
        [SerializeField] internal ContextListHandler button_settings;
        [SerializeField] ScrollRect scrollview;
        [SerializeField] internal RectTransform rt_body, rt_dragzones;
        [SerializeField] TabHeader prefab_tabHeader;
        public readonly List<SguiFrame> frames = new();
        internal readonly ValueNotifier<TabHeader> current_tab = new();
        [AutoStaticsCleanup] internal static readonly ValueNotifier<TabHeader> current_drag = new();

        //--------------------------------------------------------------------------------------------------------------

        internal void Initialize()
        {
            composer = GetComponentInParent<SguiComposer>(includeInactive: true);

            prefab_tabHeader.gameObject.SetActive(false);

            current_tab.AddListener(OnCurrentTab);

            current_drag.AddListener(OnCurrentDrag);

            button_settings.callback += (eventData, list) => composer.active_frame._value.OnTabContextList(eventData, list);
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
            header.trad_title.SetText(prefab.GetType().FullName);
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
                    SguiLoggerOverlay.Log($"SelectFrame {frame.GetArkName()} ({frame.GetType().FullName}[{frame.ark_id}])", frame);
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
            if (tab.pview == this && frames.Count == 1)
                return;

            composer.SplitView(this, dragzone).MoveTabHere(tab);
        }

        internal void MoveTabHere(TabHeader tab, TabHeader beside = null, bool after = false)
        {
            if (tab == beside)
                return;

            var source = tab.pview;
            var frame = tab.frame;
            int frameIndex = beside == null ? frames.Count : frames.IndexOf(beside.frame) + (after ? 1 : 0);
            int siblingIndex = beside == null ? prefab_tabHeader.transform.parent.childCount : beside.transform.GetSiblingIndex() + (after ? 1 : 0);

            if (source == this)
            {
                // Both indices still include the tab being moved.
                if (frames.IndexOf(frame) < frameIndex)
                    frameIndex--;
                if (tab.transform.GetSiblingIndex() < siblingIndex)
                    siblingIndex--;
                frames.Remove(frame);
            }

            current_drag.Value = null;
            composer.CancelCloseRequest();
            source.composer.CancelCloseRequest();
            frame.isFocused.Value = false;
            frame.pview = tab.pview = this;
            tab.transform.SetParent(prefab_tabHeader.transform.parent, false);
            tab.transform.SetSiblingIndex(siblingIndex);
            frame.transform.SetParent(rt_body, false);
            frame.transform.AsRTfm().FillParent();
            frame.canvas = frame.GetComponentInParent<Canvas>(true);
            frame.raycaster = frame.GetComponentInParent<GraphicRaycaster>(true);
            frames.Insert(frameIndex, frame);
            if (source != this)
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
