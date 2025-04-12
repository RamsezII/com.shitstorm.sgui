using _ARK_;
using _UTIL_;
using System;
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

        public Action<BaseStates> onState, onState_once;
        public Action onDestroy;

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
            NUCLEOR.delegates.onLateUpdate += UpdateHue;

            IMGUI_global.instance.users_ongui.RemoveElement(OnIMGui_toggle_fullscreen);
            IMGUI_global.instance.users_ongui.AddElement(OnIMGui_toggle_fullscreen, this);
        }

        protected virtual void OnDisable()
        {
            NUCLEOR.delegates.onLateUpdate -= UpdateHue;
            IMGUI_global.instance.users_ongui.RemoveElement(OnIMGui_toggle_fullscreen);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Start()
        {
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