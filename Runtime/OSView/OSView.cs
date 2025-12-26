using _ARK_;
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

        [HideInInspector] public Animator animator;
        public readonly ListListener users = new();

        [HideInInspector] public CanvasGroup canvasGroup;
        [HideInInspector] public GraphicRaycaster graphicRaycaster;

        TextMeshProUGUI text_computer_time;
        HeartBeat.Operation refresh_computer_time_operation;

        RectTransform header_rt, taskbar_rt;
        [HideInInspector] public RectTransform windows_rt;

        public RectTransform rt_editor_buttons;
        public Button edit_play, edit_pause, edit_close;

        [SerializeField] SoftwareButton prefab_softwarebutton;
        public readonly Dictionary<Type, SoftwareButton> softwaresButtons = new();

        static readonly object auto_usage = new();
        public void ToggleSelf(in bool toggle) => users.ToggleElement(auto_usage, toggle);

        readonly object timestopUser = new();

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;

            animator = GetComponent<Animator>();
            animator.keepAnimatorStateOnDisable = true;
            animator.writeDefaultValuesOnDisable = true;

            canvasGroup = GetComponent<CanvasGroup>();
            graphicRaycaster = GetComponent<GraphicRaycaster>();

            header_rt = (RectTransform)transform.Find("header");
            taskbar_rt = (RectTransform)transform.Find("task-bar");
            windows_rt = (RectTransform)transform.Find("windows");

            text_computer_time = transform.Find("task-bar/buttons-right/time/text").GetComponent<TextMeshProUGUI>();

            rt_editor_buttons = (RectTransform)transform.Find("header/buttons-central");
            edit_play = rt_editor_buttons.Find("layout/play").GetComponent<Button>();
            edit_pause = rt_editor_buttons.Find("layout/pause").GetComponent<Button>();
            edit_close = rt_editor_buttons.Find("layout/close").GetComponent<Button>();

            prefab_softwarebutton = transform.Find("task-bar/buttons-left/_SGUI_.SoftwareButton").GetComponent<SoftwareButton>();

            transform.Find("clickable").GetComponent<PointerClickHandler>().onClick += (PointerEventData eventData) =>
            {
                users.RemoveElement(auto_usage);
            };

            SguiMonitor.AddSoftwareButton();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            transform.Find("task-bar/main-button").GetComponent<Button>().onClick.AddListener(OSMainMenu.instance.Toggle);

            prefab_softwarebutton.gameObject.SetActive(false);

            NUCLEOR.instance.heartbeat_unscaled.AddOperation(new("refresh datetime", 4, true, () =>
            {
                if (text_computer_time.gameObject.activeInHierarchy)
                    RefreshDatetime();
            })
            {
                delay = 15,
            });

            users.AddListener1(toggle =>
            {
                if (this == null)
                    return;

                BaseStates state = state_base;
                float fade = 0, offset = 0;

                switch (state_base)
                {
                    case BaseStates.Default:
                        if (toggle)
                            state = BaseStates.Enable;
                        break;

                    case BaseStates.Enable:
                        if (!toggle)
                        {
                            state = BaseStates.Enable_;
                            offset = 1 - animator.GetNormalizedTimeClamped();
                        }
                        break;

                    case BaseStates.Enable_:
                        if (toggle)
                        {
                            state = BaseStates.Enable;
                            offset = 1 - animator.GetNormalizedTimeClamped();
                        }
                        break;
                }

                if (state != state_base)
                    animator.CrossFade((int)state, fade, (int)AnimLayers.Base, offset);
            });

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
                french = $"Éteindre {Application.productName} ?",
                english = $"Shutdown {Application.productName} ?",
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

        void ResizeSguiGlobal2DrT()
        {
            RectTransform rT = SguiGlobal.instance.rT_2D;

            if (state_base == BaseStates.Default)
            {
                rT.anchoredPosition = new(.5f, 0);
                rT.sizeDelta = new(0, 0);
            }
            else
            {
                float top_h = header_rt.rect.height;
                float bottom_h = taskbar_rt.rect.height;

                rT.anchoredPosition = new(.5f, bottom_h);
                rT.sizeDelta = new(0, 1 - top_h - bottom_h);
            }
        }
    }
}