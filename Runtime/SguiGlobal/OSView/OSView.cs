using _ARK_;
using _UTIL_;
using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public partial class OSView : MonoBehaviour
    {
        public static OSView instance;

        [HideInInspector] public Animator animator;
        public readonly ListListener users = new();

        TextMeshProUGUI text_computer_time;
        HeartBeat.Operation refresh_computer_time_operation;

        RectTransform header_rt, taskbar_rt;
        [HideInInspector] public RectTransform windows_rt;

        public RectTransform rt_editor_buttons;
        public Button eb_play, eb_pause, eb_close;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;

            animator = GetComponent<Animator>();
            animator.keepAnimatorStateOnDisable = true;
            animator.writeDefaultValuesOnDisable = true;

            header_rt = (RectTransform)transform.Find("header");
            taskbar_rt = (RectTransform)transform.Find("task-bar");
            windows_rt = (RectTransform)transform.Find("windows");

            text_computer_time = transform.Find("task-bar/buttons-right/time/text").GetComponent<TextMeshProUGUI>();

            rt_editor_buttons = (RectTransform)transform.Find("header/buttons-central");
            eb_play = rt_editor_buttons.Find("layout/play").GetComponent<Button>();
            eb_pause = rt_editor_buttons.Find("layout/pause").GetComponent<Button>();
            eb_close = rt_editor_buttons.Find("layout/close").GetComponent<Button>();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnEnable()
        {
            RefreshDatetime();
            UsageManager.AddUser(this, UsageGroups.BlockPlayer, UsageGroups.GameMouse);
        }

        private void OnDisable()
        {
            UsageManager.RemoveUser(this);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            transform.Find("task-bar/main-button").GetComponent<Button>().onClick.AddListener(OSMainMenu.instance.Toggle);

            NUCLEOR.instance.heartbeat_unscaled.operations.Add(new(4, true, () =>
            {
                if (text_computer_time.gameObject.activeInHierarchy)
                    RefreshDatetime();
            })
            {
                delay = 15,
            });

            users.AddListener1(this, toggle =>
            {
                BaseStates state = state_base;
                float fade = 0, offset = 0;

                switch (state_base)
                {
                    case BaseStates.Default:
                        if (toggle)
                        {
                            gameObject.SetActive(true);
                            state = BaseStates.Enable;
                        }
                        break;

                    case BaseStates.Enable:
                        if (!toggle)
                        {
                            state = BaseStates.Enable_;
                            offset = 1 - animator.GetNormlizedTimeClamped();
                        }
                        break;

                    case BaseStates.Enable_:
                        if (toggle)
                        {
                            state = BaseStates.Enable;
                            offset = 1 - animator.GetNormlizedTimeClamped();
                        }
                        break;
                }

                if (state != state_base)
                    animator.CrossFade((int)state, fade, (int)AnimLayers.Base, offset);
            });
        }

        //--------------------------------------------------------------------------------------------------------------

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