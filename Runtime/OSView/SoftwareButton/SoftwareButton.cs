using _UTIL_;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public sealed partial class SoftwareButton : OSButton
    {
        internal static readonly HashSet<SoftwareButton> instances = new();

        public RectTransform rt;
        public Image img_icon, img_open, img_focus;
        public RawImage rimg_icon;
        RawImage[] rimg_instances;
        public int max_instances = 10;

        internal SguiWindow software_prefab;
        public readonly ListListener<SguiWindow> software_instances = new();

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            instances.Clear();
        }

        //--------------------------------------------------------------------------------------------------------------

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

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            rt.sizeDelta = 25 * Vector2.one;

            software_instances.AddListener2(list =>
            {
                for (int i = 0; i < rimg_instances.Length; ++i)
                    rimg_instances[i].gameObject.SetActive(i < list.Count);
            });
        }

        //--------------------------------------------------------------------------------------------------------------

        public SguiWindow InstantiateSoftware()
        {
            if (software_prefab == null)
            {
                LoggerOverlay.Log($"{nameof(software_prefab)} is null ({software_prefab})", this, logLevel: LoggerOverlay.LogLevel.Warning);
                return null;
            }

            SguiWindow instance = SguiWindow.InstantiateWindow(software_prefab, as_software: true);
            switch (instance)
            {
                case SguiWindow1 w1:
                    w1.SetScalePivot(this);
                    break;
            }
            return instance;
        }

        internal static void RefreshAllOpenStates()
        {
            foreach (var button in instances)
                button.RefreshOpenState();
        }

        internal void RefreshOpenState()
        {
            bool open = false, focus = false;

            for (int i = 0; i < software_instances._collection.Count; i++)
            {
                SguiWindow window = software_instances._collection[i];
                if (!window.oblivionized && window.isActiveAndEnabled)
                {
                    open = true;
                    if (window.HasFocus())
                        focus = true;

                    if (open && focus)
                        break;
                }
            }

            img_open.gameObject.SetActive(open);
            img_focus.gameObject.SetActive(focus);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnDestroy()
        {
            instances.Remove(this);

            if (software_prefab != null)
                foreach (SguiWindow instance in FindObjectsByType(software_prefab.GetType(), FindObjectsInactive.Include, FindObjectsSortMode.None))
                    if (instance != null)
                        Destroy(instance.gameObject);
        }
    }
}