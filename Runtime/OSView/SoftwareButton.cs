using _ARK_;
using _UTIL_;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_
{
    public class SoftwareButton : OSButton, IPointerClickHandler
    {
        internal static readonly HashSet<SoftwareButton> instances = new();

        public RectTransform rt;
        TMP_Dropdown dropdown;
        public Image img_icon, img_open, img_focus;
        public RawImage rimg_icon;
        RawImage[] img_instances;
        public int max_instances = 10;

        internal SguiWindow software_prefab;
        public readonly ListListener<SguiWindow> software_instances = new();

        public Action<PointerEventData> onClick_left_empty, onClick_middle;
        public Func<PointerEventData, bool> onClick_left_notEmpty, onClick_right;

        public readonly ListListener<(Traductions trad, Action action)> dropdownOptions = new();

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
            dropdown = transform.Find("dropdown").GetComponent<TMP_Dropdown>();
            img_instances = transform.Find("active").GetComponentsInChildren<RawImage>(true);
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
                for (int i = 0; i < img_instances.Length; i++)
                    img_instances[i].gameObject.SetActive(list.Count > i);

                dropdownOptions.Modify(_list =>
                {
                    _list.Clear();

                    _list.Add((
                        trad: new Traductions()
                        {
                            french = $"Ajouter une fenêtre",
                            english = $"Add a new window",
                        },
                        action: () =>
                        {
                            InstantiateSoftware();
                        }
                    ));

                    for (int i = 0; i < software_instances._collection.Count; i++)
                    {
                        SguiWindow window = software_instances._collection[i];
                        if (window.trad_title == null)
                            LoggerOverlay.Log($"error trad: {window}", window, logLevel: LoggerOverlay.LogLevel.Warning);
                        else
                            _list.Add((
                                trad: window.sgui_description._value,
                                action: () =>
                                {
                                    window.SetScalePivot(this);
                                    window.ToggleWindow(true);
                                    window.TakeFocus();
                                }
                            ));
                    }

                    _list.Add((
                        trad: new Traductions()
                        {
                            french = $"Fermer toutes les fenêtres",
                            english = $"Close all windows",
                        },
                        action: () =>
                        {
                            for (int i = software_instances._collection.Count - 1; i >= 0; --i)
                            {
                                SguiWindow window = software_instances._collection[i];
                                window.SetScalePivot(null);
                                window.Oblivionize();
                            }
                        }
                    ));
                });

                RefreshOpenState();
            });

            Traductable.language.AddListener(OnTraductablesLangage);

            dropdownOptions.AddListener2(_list =>
            {
                dropdown.ClearOptions();
                List<string> options = _list.Select(x => x.trad.Automatic).ToList();
                dropdown.AddOptions(options);
            });
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnTraductablesLangage(Languages value)
        {
            dropdownOptions.Modify(null);
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
                    {
                        if (onClick_left_empty != null)
                            onClick_left_empty(eventData);
                        else
                            InstantiateSoftware();
                    }
                    else
                    {
                        if (onClick_left_notEmpty == null || onClick_left_notEmpty(eventData))
                            if (software_instances._collection.Count == 1)
                            {
                                SguiWindow instance = software_instances._collection[0];
                                instance.SetScalePivot(this);

                                if (instance.HasFocus())
                                    instance.ToggleWindow();
                                else
                                    instance.TakeFocus();
                            }
                    }
                    break;

                case PointerEventData.InputButton.Middle:
                    onClick_middle?.Invoke(eventData);
                    break;

                case PointerEventData.InputButton.Right:
                    if (onClick_right == null || onClick_right(eventData))
                        if (dropdownOptions._collection.Count > 0)
                        {
                            dropdown.Show();
                            var toggles = dropdown.GetComponentsInChildren<Toggle>(false);
                            for (int i = 0; i < toggles.Length; ++i)
                            {
                                var click_handler = toggles[i].GetComponent<PointerClickHandler>();
                                var pair = dropdownOptions._collection[i];
                                click_handler.onClick = (PointerEventData eventData) => pair.action();
                            }
                        }
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
            instances.Remove(this);

            Traductable.language.RemoveListener(OnTraductablesLangage);

            if (software_prefab != null)
                foreach (SguiWindow instance in FindObjectsByType(software_prefab.GetType(), FindObjectsInactive.Include, FindObjectsSortMode.None))
                    if (instance != null)
                        Destroy(instance.gameObject);
        }
    }
}