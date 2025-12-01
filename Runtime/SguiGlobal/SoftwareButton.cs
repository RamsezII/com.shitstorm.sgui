using _UTIL_;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_
{
    public class SoftwareButton : OSButton, IPointerClickHandler
    {
        [HideInInspector] public RectTransform rt;
        [HideInInspector] public Button button;
        [HideInInspector] public TMP_Dropdown dropdown;
        [HideInInspector] public Image img_icon;
        [HideInInspector] public RawImage rimg_icon;
        RawImage[] img_instances;

        internal SguiWindow software_prefab;
        public readonly ListListener<SguiWindow> software_instances = new();

        public Action<PointerEventData> onClick_left_empty, onClick_middle;
        public Func<PointerEventData, bool> onClick_left_notEmpty, onClick_right;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            rt = (RectTransform)transform;
            button = GetComponent<Button>();
            dropdown = transform.Find("dropdown").GetComponent<TMP_Dropdown>();
            img_instances = transform.Find("active").GetComponentsInChildren<RawImage>(true);
            img_icon = transform.Find("img_icon")?.GetComponent<Image>();
            rimg_icon = transform.Find("rimg_icon").GetComponent<RawImage>();

            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            rt.sizeDelta = 25 * Vector2.one;

            if (software_prefab != null)
                software_instances.AddListener2(this, list =>
                {
                    for (int i = 0; i < img_instances.Length; i++)
                        img_instances[i].gameObject.SetActive(list.Count > i);
                });
        }

        //--------------------------------------------------------------------------------------------------------------

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (software_prefab == null)
            {
                LoggerOverlay.Log($"{nameof(software_prefab)} is null ({software_prefab})", this, logLevel: LoggerOverlay.LogLevel.Warning);
                return;
            }

            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    if (software_instances.IsEmpty)
                        if (onClick_left_empty != null)
                            onClick_left_empty(eventData);
                        else
                            InstantiateSoftware();
                    else if (onClick_left_notEmpty == null || onClick_left_notEmpty(eventData))
                    {
                        for (int i = 0; i < software_instances._collection.Count; i++)
                        {
                            SguiWindow instance = software_instances._collection[i];
                            instance.SetScalePivot(this);
                            instance.ToggleWindow(true);
                        }
                    }
                    break;

                case PointerEventData.InputButton.Middle:
                    onClick_middle?.Invoke(eventData);
                    break;

                case PointerEventData.InputButton.Right:
                    if (onClick_right == null || onClick_right(eventData))
                        dropdown.Show();
                    break;
            }
        }

        public SguiWindow InstantiateSoftware()
        {
            SguiWindow instance = SguiWindow.InstantiateWindow(software_prefab);
            switch (instance)
            {
                case SguiWindow1 w1:
                    w1.SetScalePivot(this);
                    break;
            }
            return instance;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnDestroy()
        {
            if (software_prefab != null)
                foreach (SguiWindow instance in FindObjectsByType(software_prefab.GetType(), FindObjectsInactive.Include, FindObjectsSortMode.None))
                    if (instance != null)
                        Destroy(instance.gameObject);
        }
    }
}