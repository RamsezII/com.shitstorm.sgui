using _ARK_;
using _SGUI_.composer;
using _SGUI_.osview;
using _UTIL_;
using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public sealed partial class OSView : MonoBehaviour
    {
        [AutoStaticsCleanup] public static OSView instance;

        public CanvasGroup rootGroup;

        [SerializeField] TMP_Text text_computer_time;

        public RectTransform
            header_rt, rt_header_persistent,
            taskbar_rt, rt_footer_persistent,
            rt_unfocused_text,
            rt_unfocused_overlay,
            rt_editor,
            rt_editor_buttons,
            rt_softwares,
            vchat_icon_rT, vchat_bar_rT;

        public Button edit_play, edit_pause, edit_close;
        public BackgroundReceiver bg_receiver;
        [SerializeField] TMP_Text text_framerate;
        [SerializeField] OSHeaderButton prefab_headerbutton;
        [SerializeField] SoftwareButton prefab_softwarebutton;
        public readonly Dictionary<Type, SoftwareButton> softwaresButtons = new();

        readonly object timestopUser = new();

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;

            foreach (OSButton button in GetComponentsInChildren<OSButton>(true))
                osbuttons_prefabs[button.GetType()] = button;
            osbuttons_prefabs[typeof(SoftwareButton)] = prefab_softwarebutton;

            AwakeToggle();
            AwakeSguiSettings();
            AwakeRuntimeSettings();

            isVisible.AddListener(rt_softwares.gameObject.SetActive);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            bg_receiver.onPointerClick_data += _ => ToggleSelf(false);

            toggle.AddListener(value => bg_receiver.raycastReceiver.raycastTarget = value);

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
                SguiCustom.ShowAlert(SguiDialogs.Dialog, out _, new()
                {
                    french = $"Éteindre {Application.productName.Bold()} ?",
                    english = $"Power off {Application.productName.Bold()}?",
                }).onAction_confirm += () => NUCLEOR.ShutdownApplication();
            });

            edit_pause.onClick.AddListener(() => NUCLEOR.instance.timeScale_raw.Value = NUCLEOR.instance.timeScale_raw._value > 0 ? 0 : 1);

            NUCLEOR.instance.timeScale_raw.AddListener(value =>
            {
                bool timestop = value <= 0;
                edit_pause.transform.Find("toggle").gameObject.SetActive(timestop);
                users_forceOpen.ToggleElement(timestopUser, timestop);
            });

            StartButtons();

            NUCLEOR.instance.isFocused.AddListener(isFocused =>
            {
                rt_unfocused_text.gameObject.SetActive(!isFocused);
                if (false)
                    rt_unfocused_overlay.gameObject.SetActive(!isFocused);
            });

            foreach (var type in Util.EGetAllDerivedTypes<SguiFrame>())
            {
                var prefab = Resources.Load<SguiFrame>(type.FullName);
                if (prefab == null)
                    Debug.LogWarning($"no resource with name \"{type}\"", this);
                else
                    AddSoftwareButton(prefab);
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        public OSHeaderButton AddHeaderButton() => prefab_headerbutton.Clone(true);

        public SoftwareButton AddSoftwareButton<T>(in Traductions hoverInfos) where T : SguiFrame => AddSoftwareButton(typeof(T), hoverInfos);
        public SoftwareButton AddSoftwareButton(in SguiFrame frame) => AddSoftwareButton(frame.GetType(), frame.sgui_description);
        public SoftwareButton AddSoftwareButton(in Type type, in Traductions hoverInfos)
        {
            if (softwaresButtons.TryGetValue(type, out SoftwareButton button))
                Debug.LogWarning($"already instantiated \"{type}\"", this);
            else
            {
                SguiFrame prefab = (SguiFrame)Util.LoadResourceByType(type);
                if (prefab == null)
                    Debug.LogError($"{this}: Failed to load software prefab of type '{type}'.", this);
                else
                {
                    softwaresButtons[type] = button = Instantiate(prefab_softwarebutton, prefab_softwarebutton.transform.parent);
                    button.Initialize();
                    button.hover_info = hoverInfos;
                    button.rimg_icon.texture = prefab.window_icon;
                    button.frame_prefab = prefab;
                    button.gameObject.SetActive(true);
                }
            }
            return button;
        }

        public SoftwareButton AddSoftwareButton(in Traductions hoverInfos, in Texture icon)
        {
            var button = Instantiate(prefab_softwarebutton, prefab_softwarebutton.transform.parent);
            button.Initialize();
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
