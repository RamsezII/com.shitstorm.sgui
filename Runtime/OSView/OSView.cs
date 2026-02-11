using _ARK_;
using _SGUI_.osview;
using _UTIL_;
using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_
{
    public partial class OSView : MonoBehaviour
    {
        public static OSView instance;

        [HideInInspector] public CanvasGroup canvasGroup;
        [HideInInspector] public GraphicRaycaster graphicRaycaster;

        TextMeshProUGUI text_computer_time;
        HeartBeat.Operation refresh_computer_time_operation;

        RectTransform header_rt, taskbar_rt;
        [HideInInspector] public RectTransform rt_editor, rt_softwares;

        public RectTransform rt_editor_buttons;
        public Button edit_play, edit_pause, edit_close;

        [SerializeField] OSHeaderButton prefab_headerbutton;
        [SerializeField] SoftwareButton prefab_softwarebutton;
        public readonly Dictionary<Type, SoftwareButton> softwaresButtons = new();

        public OSHeaderButton AddHeaderButton() => prefab_headerbutton.Clone(true);

        readonly object timestopUser = new();

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;

            canvasGroup = GetComponent<CanvasGroup>();
            graphicRaycaster = GetComponent<GraphicRaycaster>();

            header_rt = (RectTransform)transform.Find("header");
            taskbar_rt = (RectTransform)transform.Find("task-bar");
            rt_editor = (RectTransform)transform.Find("windows/editor-layer");
            rt_softwares = (RectTransform)transform.Find("windows/softwares-layer");

            text_computer_time = transform.Find("task-bar/buttons-right/time/text").GetComponent<TextMeshProUGUI>();

            rt_editor_buttons = (RectTransform)transform.Find("header/buttons-central");
            edit_play = rt_editor_buttons.Find("layout/play").GetComponent<Button>();
            edit_pause = rt_editor_buttons.Find("layout/pause").GetComponent<Button>();
            edit_close = rt_editor_buttons.Find("layout/close").GetComponent<Button>();

            prefab_softwarebutton = transform.Find("task-bar/buttons-left/_SGUI_.SoftwareButton").GetComponent<SoftwareButton>();

            prefab_headerbutton = GetComponentInChildren<OSHeaderButton>(true);

            transform.Find("clickable").GetComponent<PointerClickHandler>().onClick += (PointerEventData eventData) =>
            {
                users.RemoveElement(auto_usage);
            };

            AwakeToggle();

            SguiMonitor.AddSoftwareButton();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            transform.Find("task-bar/main-button").GetComponent<Button>().onClick.AddListener(OSMainMenu.instance.Toggle);

            prefab_headerbutton.gameObject.SetActive(false);
            prefab_softwarebutton.gameObject.SetActive(false);

            NUCLEOR.instance.heartbeat_unscaled.AddOperation(new("refresh datetime", 4, true, () =>
            {
                if (text_computer_time.gameObject.activeInHierarchy)
                    RefreshDatetime();
            })
            {
                delay = 15,
            });

            StartToggle();

            edit_play.onClick.AddListener(() => ToggleSelf(false));
            edit_close.onClick.AddListener(ShowApplicationShutdownConfirm);
            edit_pause.onClick.AddListener(() => NUCLEOR.instance.timeScale_raw.Value = NUCLEOR.instance.timeScale_raw._value > 0 ? 0 : 1);

            NUCLEOR.instance.timeScale_raw.AddListener(value =>
            {
                bool timestop = value <= 0;
                edit_pause.transform.Find("toggle").gameObject.SetActive(timestop);
                users.ToggleElement(timestopUser, timestop);
            });

            Application.wantsToQuit += () =>
            {
                if (ArkMachine.flag_shutdown)
                    return true;
                ShowApplicationShutdownConfirm();
                return false;
            };
        }

        //--------------------------------------------------------------------------------------------------------------

        public SoftwareButton AddSoftwareButton<T>(in Traductions hoverInfos) where T : SguiWindow => AddSoftwareButton(typeof(T), hoverInfos);
        public SoftwareButton AddSoftwareButton(in Type type, in Traductions hoverInfos)
        {
            if (!softwaresButtons.TryGetValue(type, out SoftwareButton button) || button == null)
            {
                SguiWindow prefab = (SguiWindow)Util.LoadResourceByType(type);
                if (prefab == null)
                    Debug.LogError($"{this}: Failed to load software prefab of type '{type}'.", this);
                else
                {
                    softwaresButtons[type] = button = Instantiate(prefab_softwarebutton, prefab_softwarebutton.transform.parent);
                    button.hover_info = hoverInfos;
                    button.rimg_icon.texture = prefab.window_icon;
                    button.software_prefab = prefab;
                    button.gameObject.SetActive(true);
                }
            }
            return button;
        }

        public SoftwareButton AddSoftwareButton(in Traductions hoverInfos, in Texture icon)
        {
            var button = Instantiate(prefab_softwarebutton, prefab_softwarebutton.transform.parent);
            button.hover_info = hoverInfos;
            button.rimg_icon.texture = icon;
            button.gameObject.SetActive(true);
            return button;
        }

        public static void ShowApplicationShutdownConfirm()
        {
            var dialog = SguiWindow.ShowAlert(SguiDialogs.Dialog, out _, new()
            {
                french = $"Fermer {Application.productName} ?",
                english = $"Close {Application.productName} ?",
            });
            dialog.onAction_confirm += () => ArkMachine.ShutdownApplication(true);
        }

        void RefreshDatetime()
        {
            DateTime now = DateTime.Now;
            string time = now.ToString("HH:mm", CultureInfo.CurrentCulture);
            string date = now.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture);
            text_computer_time.text = $"{time}\n{date}";
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnDestroy()
        {
            NUCLEOR.delegates.LateUpdate -= RefreshToggle;
        }
    }
}