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
        [HideInInspector] public RectTransform rt;
        [HideInInspector] public Button button;
        [HideInInspector] TMP_Dropdown dropdown;
        [HideInInspector] public Image img_icon, img_open;
        [HideInInspector] public RawImage rimg_icon;
        RawImage[] img_instances;

        internal SguiWindow software_prefab;
        public readonly ListListener<SguiWindow> software_instances = new();

        public Action<PointerEventData> onClick_left_empty, onClick_middle;
        public Func<PointerEventData, bool> onClick_left_notEmpty, onClick_right;

        readonly Dictionary<string, Action> onDropdownButtons = new(StringComparer.OrdinalIgnoreCase);

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            rt = (RectTransform)transform;
            button = GetComponent<Button>();
            dropdown = transform.Find("dropdown").GetComponent<TMP_Dropdown>();
            img_instances = transform.Find("active").GetComponentsInChildren<RawImage>(true);
            img_icon = transform.Find("img_icon").GetComponent<Image>();
            img_open = transform.Find("is-open").GetComponent<Image>();
            rimg_icon = transform.Find("rimg_icon").GetComponent<RawImage>();

            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            rt.sizeDelta = 25 * Vector2.one;

            software_instances.AddListener2(this, list =>
            {
                for (int i = 0; i < img_instances.Length; i++)
                    img_instances[i].gameObject.SetActive(list.Count > i);
                RefreshDropdown();
            });

            RefreshOpenState();

            Traductable.language.AddListener(this, value => RefreshDropdown());

            dropdown.onValueChanged.AddListener(index =>
            {
                if (dropdown.TryGetSelectedValue(out string name))
                    if (onDropdownButtons.TryGetValue(name, out Action action))
                        if (action != null)
                            action();
                        else
                            Debug.LogWarning($"{nameof(action)} for button {software_prefab.GetType()} is null");
                    else
                        Debug.LogWarning($"no option \"{name}\" registered for button {software_prefab.GetType()}");
                else
                    Debug.LogWarning($"can not identify dropdown option selected by button {software_prefab.GetType()}");
            });
        }

        //--------------------------------------------------------------------------------------------------------------

        void RefreshDropdown()
        {
            onDropdownButtons.Clear();
            dropdown.ClearOptions();

            AddOption(
                key: new Traductions()
                {
                    french = $"Ajouter une fenêtre",
                    english = $"Add a new window",
                }.Automatic,
                action: null
            );

            for (int i = 0; i < software_instances._collection.Count; i++)
            {
                SguiWindow window = software_instances._collection[i];
                if (window.trad_title == null)
                    LoggerOverlay.Log($"error trad: {window}", window, logLevel: LoggerOverlay.LogLevel.Warning);
                else
                    AddOption($"[{window.id}] \"{window.trad_title.traductions.Automatic}\"", () =>
                    {
                        window.SetScalePivot(this);
                        window.ToggleWindow(true);
                        window.TakeFocus();
                    });
            }

            AddOption(
                key: new Traductions()
                {
                    french = $"Fermer toutes les fenêtres",
                    english = $"Close all windows",
                }.Automatic,
                action: () =>
                {
                    for (int i = software_instances._collection.Count - 1; i >= 0; --i)
                    {
                        SguiWindow window = software_instances._collection[i];
                        window.SetScalePivot(null);
                        window.Oblivionize();
                    }
                }
            );
        }

        void AddOption(in string key, in Action action)
        {
            onDropdownButtons.Add(key, action);
            dropdown.AddOptions(new List<string>() { key, });
        }

        internal void RefreshOpenState()
        {
            bool open = false;

            for (int i = 0; i < software_instances._collection.Count; i++)
            {
                SguiWindow window = software_instances._collection[i];
                if (!window.oblivionized && window.isActiveAndEnabled)
                    open = true;
            }

            img_open.gameObject.SetActive(open);
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
                        if (dropdown.options.Count > 0)
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
            Traductable.language._propagator.RemoveListener_user(this);
            if (software_prefab != null)
                foreach (SguiWindow instance in FindObjectsByType(software_prefab.GetType(), FindObjectsInactive.Include, FindObjectsSortMode.None))
                    if (instance != null)
                        Destroy(instance.gameObject);
        }
    }
}