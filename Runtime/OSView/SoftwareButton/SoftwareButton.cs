using _ARK_;
using _SGUI_.composer;
using _UTIL_;
using System.Collections.Generic;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public sealed partial class SoftwareButton : OSButton, SguiContextHover.IUser
    {
        [AutoStaticsCleanup] internal static readonly HashSet<SoftwareButton> instances = new();
        public RectTransform rt;
        public Image img_icon, img_open, img_focus;
        public RawImage rimg_icon;
        RawImage[] rimg_instances;
        public int max_instances = 10;
        public Traductions hover_info;
        Traductions SguiContextHover.IUser.OnSguiContextHover() => hover_info;
        internal SguiFrame frame_prefab;
        public readonly ListListener<SguiFrame> users = new();

        protected override void Awake()
        {
            instances.Add(this);
            rt = (RectTransform)transform;
            rimg_instances = transform.Find("active").GetComponentsInChildren<RawImage>(true);
            img_icon = transform.Find("img_icon").GetComponent<Image>();
            img_open = transform.Find("is-open").GetComponent<Image>();
            img_focus = transform.Find("has-focus").GetComponent<Image>();
            rimg_icon = transform.Find("rimg_icon").GetComponent<RawImage>();
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            rt.sizeDelta = 25 * Vector2.one;
            users.AddListener2(list =>
            {
                for (int i = 0; i < rimg_instances.Length; ++i) rimg_instances[i].gameObject.SetActive(i < list.Count);
                RefreshOpenState();
            });
        }

        public SguiFrame InstantiateSoftware() => InstantiateSoftware(false);
        public SguiFrame InstantiateSoftware(bool new_window)
        {
            if (frame_prefab == null)
            {
                SguiLoggerOverlay.Log($"{nameof(frame_prefab)} is null", this, logLevel: SguiLogLevel.Warning);
                return null;
            }
            return OSView.InstantiateSoftware(frame_prefab, new_window);
        }

        internal static void RefreshAllOpenStates() => Util.AddActionOnce(ref NUCLEOR.delegates.LateUpdate_onEndOfFrame_once, RefreshAllOpenStates_now);
        internal static void RefreshAllOpenStates_now()
        {
            foreach (var button in instances) if (button != null) button.RefreshOpenState();
        }

        internal void RefreshOpenState()
        {
            bool open = false, focus = false;
            foreach (var frame in users._collection)
                if (frame != null && !frame.oblivionized)
                {
                    open = true;
                    focus |= frame.isFocused._value;
                }
            img_open.gameObject.SetActive(open);
            img_focus.gameObject.SetActive(focus);
        }

        private void OnDestroy()
        {
            instances.Remove(this);
            foreach (var frame in users._collection.ToArray())
                if (frame != null)
                    frame.Oblivionize();
            users.Clear();
        }
    }
}
