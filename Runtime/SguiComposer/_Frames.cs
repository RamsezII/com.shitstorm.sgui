using _SGUI_.composer;
using _UTIL_;
using System;
using System.Linq;
using UnityEngine;

namespace _SGUI_
{
    partial class SguiComposer
    {
        [SerializeField] SguiSplitView[] splitviews;
        public readonly ValueNotifier<SguiFrame> active_frame = new();
        public SguiSplitView active_splitview => active_frame._value?.pview ?? splitviews.FirstOrDefault(v => v != null);

        //--------------------------------------------------------------------------------------------------------------

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
            var rt_group = (RectTransform)new GameObject("SplitGroup", typeof(RectTransform)).transform;
            rt_group.SetParent(rt.parent, false);
            rt_group.SetSiblingIndex(rt.GetSiblingIndex());
            rt_group.anchorMin = rt.anchorMin;
            rt_group.anchorMax = rt.anchorMax;
            rt_group.offsetMin = rt.offsetMin;
            rt_group.offsetMax = rt.offsetMax;

            rt.SetParent(rt_group, false);
            rt.anchorMin = dragzone.opposite_dragzone.rt_zone.anchorMin;
            rt.anchorMax = dragzone.opposite_dragzone.rt_zone.anchorMax;
            rt.offsetMin = rt.offsetMax = Vector2.zero;

            return AddSplitView(dragzone.rt_zone.anchorMin, dragzone.rt_zone.anchorMax, rt_group);
        }

        internal void RemoveSplitView(SguiSplitView view)
        {
            splitviews = splitviews.Where(v => v != view).ToArray();
            var rt_group = view.transform.parent.AsRTfm();
            view.gameObject.SetActive(false);
            // Destroy is deferred; detach now to keep child counts correct this frame.
            view.transform.SetParent(null, false);
            Destroy(view.gameObject);

            if (rt_group != null && rt_group != rt_body && rt_group.childCount == 1)
            {
                var sibling = rt_group.GetChild(0).AsRTfm();
                sibling.SetParent(rt_group.parent, false);
                sibling.SetSiblingIndex(rt_group.GetSiblingIndex());
                sibling.anchorMin = rt_group.anchorMin;
                sibling.anchorMax = rt_group.anchorMax;
                sibling.offsetMin = rt_group.offsetMin;
                sibling.offsetMax = rt_group.offsetMax;
                rt_group.SetParent(null, false);
                Destroy(rt_group.gameObject);
            }

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
    }
}