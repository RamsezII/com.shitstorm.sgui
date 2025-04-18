using _ARK_;
using _UTIL_;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace _SGUI_
{
    public abstract partial class SguiWindow : MonoBehaviour
    {
        public static readonly ListListener<SguiWindow> instances = new();
        public bool HasFocus => instances.IsLast(this);

        [HideInInspector] public Animator animator;

        [SerializeField] bool animated_toggle = true;

        public readonly OnValue<bool> sgui_toggle_window = new();

        public Action<BaseStates, bool> onState, onState_once;
        public Action onDestroy;

        [SerializeField]
        bool
            animate_hue = true,
            can_fullscreen,
            open_on_awake = true;

        public static T InstantiateWindow<T>() where T : SguiWindow => Util.InstantiateOrCreate<T>(SGUI_global.instance.rT);

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            animator.writeDefaultValuesOnDisable = true;
            animator.keepAnimatorStateOnDisable = true;
            animator.Update(0);
            AwakeUI();
            AwakeToggle();
        }

        protected virtual void OnEnable()
        {
            NUCLEOR.delegates.onLateUpdate -= UpdateHue;
            if (animate_hue)
                NUCLEOR.delegates.onLateUpdate += UpdateHue;

            IMGUI_global.instance.users_ongui.RemoveElement(OnIMGui_toggle_fullscreen);
            IMGUI_global.instance.users_ongui.AddElement(OnIMGui_toggle_fullscreen, this);
        }

        protected virtual void OnDisable()
        {
            NUCLEOR.delegates.onLateUpdate -= UpdateHue;
            IMGUI_global.instance.users_ongui.RemoveElement(OnIMGui_toggle_fullscreen);
        }

        private void OnApplicationFocus(bool focus)
        {
            if (focus)
                CheckBounds();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Start()
        {
            if (open_on_awake)
            {
                NUCLEOR.instance.subScheduler.AddRoutine(EOpen());
                IEnumerator<float> EOpen()
                {
                    while(state_base==0)
                        yield return 0;
                    sgui_toggle_window.Update(true);
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        bool OnIMGui_toggle_fullscreen(Event e)
        {
            if (e.type == EventType.KeyDown)
                if (e.keyCode == KeyCode.F11)
                {
                    fullscreen.Toggle();
                    return true;
                }
            return false;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void OnDestroy()
        {
            onDestroy?.Invoke();
            instances.RemoveElement(this);
            Debug.Log($"destroyed {GetType().FullName} ({transform.GetPath(true)})".ToSubLog());
        }
    }
}