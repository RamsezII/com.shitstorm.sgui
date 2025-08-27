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

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;
            animator = GetComponent<Animator>();
            animator.keepAnimatorStateOnDisable = true;
            animator.writeDefaultValuesOnDisable = true;
            text_computer_time = transform.Find("task-bar/buttons-right/time/text").GetComponent<TextMeshProUGUI>();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnEnable()
        {
            RefreshDatetime();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            transform.Find("task-bar/main-button").GetComponent<Button>().onClick.AddListener(OSMainMenu.instance.Toggle);
            users.AddListener1(this, ToggleView);

            refresh_computer_time_operation = new(4, false, delay =>
            {
                if (text_computer_time.gameObject.activeInHierarchy)
                    RefreshDatetime();
            })
            {
                timer = 15,
            };

            NUCLEOR.instance.heartbeat_unscaled.operations.Add(refresh_computer_time_operation);
        }

        //--------------------------------------------------------------------------------------------------------------

        void RefreshDatetime()
        {
            DateTime now = DateTime.Now;
            string time = now.ToString("HH:mm", CultureInfo.CurrentCulture);
            string date = now.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture);
            text_computer_time.text = $"{time}\n{date}";
        }

        public void ToggleView(bool toggle)
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
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnDestroy()
        {
            refresh_computer_time_operation.Dispose();
            if (this == instance)
                instance = null;
        }
    }
}