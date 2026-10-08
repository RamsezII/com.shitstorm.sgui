using _ARK_;
using _SGUI_.composer;
using _UTIL_;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_
{
    public sealed partial class SguiComposer : SguiWindow
    {
        [SerializeField] SguiSplitView[] splitviews;
        public readonly ValueNotifier<SguiFrame> active_frame = new();
        public readonly ValueNotifier<bool> fullscreen = new();
        [SerializeField] RectTransform rt_body;
        [SerializeField] Button button_hide, button_fullscreen;
        [SerializeField] RectTransform rt_unselected;
        bool closing, close_requested;
        public const int min_width = 200, min_height = 150;
        public SguiSplitView active_splitview => active_frame._value?.pview ?? splitviews.FirstOrDefault(v => v != null);

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnEnable()
        {
            base.OnEnable();

            if (IMGUI_global.instance != null)
                IMGUI_global.instance.inputs_users.AddElement(OnImguiInputs);
        }

        protected override void OnDisable()
        {
            if (IMGUI_global.instance != null)
                IMGUI_global.instance.inputs_users.RemoveElement(OnImguiInputs);

            RefreshFrameFocus();

            base.OnDisable();
        }

        //--------------------------------------------------------------------------------------------------------------

        internal protected override void OnInitialize()
        {
            base.OnInitialize();

            splitviews = GetComponentsInChildren<SguiSplitView>(true);

            foreach (var view in splitviews) 
                view.Initialize();

            active_frame.AddListener(_ => RefreshFrameFocus());
            fullscreen.AddListener(OnFullscreen, do_not_call_this_time: true);
            onFunc_close = RequestCloseFrames;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            var drag = rt.Find("header/header_mask/padding/drag-button").GetComponent<DragHandler>();
            drag.onBeginDrag += OnHeaderBeginDrag;
            drag.onDrag += OnHeaderDrag;
            drag.onEndDrag += OnHeaderEndDrag;

            rt.Find("header/header_mask/padding/drag-button").GetComponent<PointerClickHandler>().onClick += data =>
            {
                if (data.clickCount == 2) fullscreen.ToggleAuto();
            };

            button_fullscreen.onClick.AddListener(fullscreen.ToggleAuto);

            button_hide.onClick.AddListener(() =>
            {
                SetScalePivot(active_frame._value.os_button);
                toggle.Value = false;
            });

            toggle.AddListener(value =>
            {
                if (!value)
                    ResizerVisual.instance?.UntakeFocus(this);

                RefreshFrameFocus();
            });

            CheckBounds();
        }

        //--------------------------------------------------------------------------------------------------------------

        bool OnImguiInputs(Event e)
        {
            if (!isFocused._value || !toggle._value || e.type != EventType.KeyDown)
                return false;

            if (e.keyCode == KeyCode.F10)
            {
                CheckBounds();
                return true;
            }

            if (e.keyCode == KeyCode.F11)
            {
                fullscreen.ToggleAuto();
                return true;
            }

            return false;
        }

        void OnFullscreen(bool value)
        {
            if (value)
            {
                rect_current = new(rt);
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.sizeDelta = Vector2.zero;
                rt.anchoredPosition = Vector2.zero;
            }
            else
                rect_current.Apply(rt);

            OnResized();
        }

        protected override void OnToggleFocus(bool focus)
        {
            base.OnToggleFocus(focus);

            rt_unselected.gameObject.SetActive(!focus);
            RefreshFrameFocus();
        }

        internal void RefreshFrameFocus()
        {
            foreach (var view in splitviews)
                if (view != null)
                    foreach (var frame in view.frames)
                        if (frame != null) frame.RefreshFocus();
        }

        public SguiSplitView AddSplitView() => AddSplitView(Vector2.zero, Vector2.one);
        public SguiSplitView AddSplitView(Vector2 anchorMin, Vector2 anchorMax, RectTransform parent = null)
        {
            var view = Util.InstantiateOrCreate<SguiSplitView>(parent: parent != null ? parent : rt_body);
            view.Initialize();

            var view_rt = view.transform.AsRTfm();
            view_rt.anchorMin = anchorMin;
            view_rt.anchorMax = anchorMax;
            view_rt.offsetMin = view_rt.offsetMax = Vector2.zero;

            Array.Resize(ref splitviews, splitviews.Length + 1);
            splitviews[^1] = view;

            return view;
        }

        internal SguiSplitView SplitView(SguiSplitView view, Dragzone dragzone)
        {
            var rt = view.transform.AsRTfm();
            var group = new GameObject("SplitGroup", typeof(RectTransform)).transform.AsRTfm();
            group.SetParent(rt.parent, false);
            group.SetSiblingIndex(rt.GetSiblingIndex());
            group.anchorMin = rt.anchorMin;
            group.anchorMax = rt.anchorMax;
            group.offsetMin = rt.offsetMin;
            group.offsetMax = rt.offsetMax;

            rt.SetParent(group, false);
            rt.anchorMin = dragzone.opposite_dragzone.rt_zone.anchorMin;
            rt.anchorMax = dragzone.opposite_dragzone.rt_zone.anchorMax;
            rt.offsetMin = rt.offsetMax = Vector2.zero;

            return AddSplitView(dragzone.rt_zone.anchorMin, dragzone.rt_zone.anchorMax, group);
        }

        internal void RemoveSplitView(SguiSplitView view)
        {
            splitviews = splitviews.Where(v => v != view).ToArray();
            var group = view.transform.parent.AsRTfm();
            view.gameObject.SetActive(false);
            // Destroy is deferred; detach now to keep child counts correct this frame.
            view.transform.SetParent(null, false);

            if (group != null && group != rt_body && group.childCount == 1)
            {
                var sibling = group.GetChild(0).AsRTfm();
                sibling.SetParent(group.parent, false);
                sibling.SetSiblingIndex(group.GetSiblingIndex());
                sibling.anchorMin = group.anchorMin;
                sibling.anchorMax = group.anchorMax;
                sibling.offsetMin = group.offsetMin;
                sibling.offsetMax = group.offsetMax;
                group.SetParent(null, false);
                Destroy(group.gameObject);
            }

            Destroy(view.gameObject);
            OnResized();
        }

        internal void OnFrameRemoved(SguiFrame removed)
        {
            if (oblivionized)
                return;

            if (active_frame._value == removed)
                active_frame.Value = splitviews.Where(v => v != null).Select(v => v.current_tab._value?.frame).FirstOrDefault(f => f != null && !f.oblivionized);

            if (closing)
                return;

            if (close_requested)
            {
                OnClickClose();
                return;
            }

            if (splitviews.All(v => v == null || v.frames.Count == 0))
                Oblivionize();
        }

        internal void CancelCloseRequest() => close_requested = false;

        bool RequestCloseFrames()
        {
            close_requested = true;
            closing = true;
            try
            {
                foreach (var frame in splitviews.Where(v => v != null).SelectMany(v => v.frames).ToArray())
                    if (frame != null && !frame.RequestClose())
                        return false;
                return true;
            }
            finally
            {
                closing = false;
            }
        }

        public void CheckBounds()
        {
            if (!fullscreen._value)
            {
                var available = rt_root.rect.size;
                var size = rt.rect.size;
                rt.sizeDelta = new(Mathf.Clamp(size.x, Mathf.Min(min_width, available.x), available.x), Mathf.Clamp(size.y, Mathf.Min(min_height, available.y), available.y));
            }
            CheckPosition(out _);
            OnResized();
        }

        public void OnResized()
        {
            Canvas.ForceUpdateCanvases();
            foreach (var view in splitviews)
                if (view != null)
                    foreach (var frame in view.frames)
                        if (frame != null && frame.isActiveAndEnabled) frame.OnResized();
        }

        protected override void OnHeaderEndDrag(PointerEventData eventData)
        {
            base.OnHeaderEndDrag(eventData);
            OnResized();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnOblivion()
        {
            foreach (var frame in splitviews.Where(v => v != null).SelectMany(v => v.frames).ToArray())
                if (frame != null)
                    frame.Oblivionize();

            active_frame.Value = null;

            base.OnOblivion();
        }

        protected override void OnDestroy()
        {
            if (IMGUI_global.instance != null)
                IMGUI_global.instance.inputs_users.RemoveElement(OnImguiInputs);

            base.OnDestroy();

            active_frame.Clear();
            fullscreen.Clear();
        }
    }
}
