using _ARK_;
using _SGUI_.osview;
using _UTIL_;
using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public partial class OSView : MonoBehaviour
    {
        public static OSView instance;

        [HideInInspector] public CanvasGroup rootGroup;
        [HideInInspector] public GraphicRaycaster graphicRaycaster;

        TextMeshProUGUI text_computer_time;

        public RectTransform
            header_rt, rt_header_persistent,
            taskbar_rt, rt_footer_persistent,
            rt_unfocused,
            rt_editor,
            rt_editor_buttons,
            rt_softwares,
            vchat_icon_rT, vchat_bar_rT;

        public Button
            edit_play, edit_pause, edit_close;

        [SerializeField]
        TMP_Text
            text_framerate;

        [SerializeField] OSHeaderButton prefab_headerbutton;
        [SerializeField] SoftwareButton prefab_softwarebutton;
        public readonly Dictionary<Type, SoftwareButton> softwaresButtons = new();

        public OSHeaderButton AddHeaderButton() => prefab_headerbutton.Clone(true);

        readonly object timestopUser = new();

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void OnResetStatics()
        {
            onRuntimeSettingsPrompt.Clear();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;

            graphicRaycaster = GetComponent<GraphicRaycaster>();

            rootGroup = transform.Find("root_group").GetComponent<CanvasGroup>();

            header_rt = (RectTransform)rootGroup.transform.Find("header");
            rt_header_persistent = (RectTransform)header_rt.Find("header_persistent");
            taskbar_rt = (RectTransform)rootGroup.transform.Find("task-bar");
            rt_footer_persistent = (RectTransform)taskbar_rt.Find("footer_persistent");
            rt_unfocused = (RectTransform)transform.Find("unfocused");
            rt_editor = (RectTransform)rootGroup.transform.Find("windows/editor-layer");
            rt_softwares = (RectTransform)rootGroup.transform.Find("windows/softwares-layer");

            vchat_icon_rT = (RectTransform)rt_footer_persistent.Find("hlayout/VChat/icon");
            vchat_bar_rT = (RectTransform)vchat_icon_rT.Find("bar");

            text_computer_time = rootGroup.transform.Find("task-bar/buttons-right/time/text").GetComponent<TextMeshProUGUI>();

            rt_editor_buttons = (RectTransform)rootGroup.transform.Find("header/buttons-central");
            edit_play = rt_editor_buttons.Find("layout/play").GetComponent<Button>();
            edit_pause = rt_editor_buttons.Find("layout/pause").GetComponent<Button>();
            edit_close = rt_editor_buttons.Find("layout/close").GetComponent<Button>();

            prefab_softwarebutton = rootGroup.transform.Find("task-bar/buttons-left/_SGUI_.SoftwareButton").GetComponent<SoftwareButton>();

            prefab_headerbutton = GetComponentInChildren<OSHeaderButton>(true);

            text_framerate = rt_footer_persistent.Find("hlayout/Framerate/text").GetComponent<TextMeshProUGUI>();

            AwakeButtons();
            AwakeToggle();
            AwakeSguiSettings();
            AwakeRuntimeSettings();

            isVisible.AddListener(rt_softwares.gameObject.SetActive);

            SguiMonitor.AddSoftwareButton();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            RectTransform rt_clickable = (RectTransform)rootGroup.transform.Find("clickable");
            rt_clickable.GetComponent<PointerClickHandler>().onClick += _ => ToggleSelf(false);

            Graphic invisible_click_graphic = rt_clickable.GetComponent<Graphic>();
            toggle.AddListener(value =>
            {
                if (invisible_click_graphic != null)
                    invisible_click_graphic.raycastTarget = value;
            });

            rootGroup.transform.Find("task-bar/main-button").GetComponent<Button>().onClick.AddListener(OSMainMenu.instance.Toggle);

            prefab_headerbutton.gameObject.SetActive(false);
            prefab_softwarebutton.gameObject.SetActive(false);

            StartFramerate();

            NUCLEOR.instance.scheduler_unscaled.AddOperation(new("refresh datetime", 4, true, () =>
            {
                if (text_computer_time.gameObject.activeInHierarchy)
                    RefreshDatetime();
            })
            {
                delay = 15,
            });

            StartToggle();

            edit_play.onClick.AddListener(() => ToggleSelf(false));

            edit_close.onClick.AddListener(() =>
            {
                SguiWindow.ShowAlert(SguiDialogs.Dialog, out _, new()
                {
                    french = $"Éteindre {Application.productName.Bold()} ?",
                    english = $"Power off {Application.productName.Bold()}?",
                }).onAction_confirm += () => ArkMachine.ShutdownApplication();
            });

            edit_pause.onClick.AddListener(() => NUCLEOR.instance.timeScale_raw.Value = NUCLEOR.instance.timeScale_raw._value > 0 ? 0 : 1);

            NUCLEOR.instance.timeScale_raw.AddListener(value =>
            {
                bool timestop = value <= 0;
                edit_pause.transform.Find("toggle").gameObject.SetActive(timestop);
                users_forceOpen.ToggleElement(timestopUser, timestop);
            });

            StartButtons();

            NUCLEOR.instance.isFocused.AddListener(isFocused => rt_unfocused.gameObject.SetActive(!isFocused));
        }

        //--------------------------------------------------------------------------------------------------------------

        public SoftwareButton AddSoftwareButton<T>(in Traductions hoverInfos) where T : SguiSoftware => AddSoftwareButton(typeof(T), hoverInfos);
        public SoftwareButton AddSoftwareButton(in Type type, in Traductions hoverInfos)
        {
            if (!softwaresButtons.TryGetValue(type, out SoftwareButton button) || button == null)
            {
                SguiSoftware prefab = (SguiSoftware)Util.LoadResourceByType(type);
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

            op_framerate?.Dispose();
        }
    }
}