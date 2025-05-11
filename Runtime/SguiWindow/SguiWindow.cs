using _ARK_;
using _UTIL_;
using System;
using UnityEngine;

namespace _SGUI_
{
    public partial class SguiWindow : MonoBehaviour
    {
        public static readonly ListListener<SguiWindow> instances = new();
        public bool HasFocus => instances.IsLast(this);

        [HideInInspector] public Animator animator;

        public readonly OnValue<bool> sgui_toggle_window = new();

        public Action<BaseStates, bool> onState, onState_once;
        public Action onDestroy;

        [SerializeField]
        protected bool
            animate_hue = true,
            hide_on_close,
            open_on_awake = true;

        public bool oblivionized;

        public readonly OnValue<bool> fullscreen = new();

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Awake()
        {
            if (TryGetComponent(out animator))
            {
                animator.writeDefaultValuesOnDisable = true;
                animator.keepAnimatorStateOnDisable = true;
                animator.Update(0);
            }
            AwakeUI();
        }

        protected virtual void OnEnable()
        {
            NUCLEOR.delegates.onLateUpdate -= UpdateHue;
            if (animate_hue)
                NUCLEOR.delegates.onLateUpdate += UpdateHue;

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
            StartUI();
            StartToggle();

            if (open_on_awake)
                NUCLEOR.instance.subScheduler.AddRoutine(Util.EWaitUntil(() => state_base != 0, null, () => sgui_toggle_window.Update(true)));

            UsageManager.ToggleUser(this, true, UsageGroups.Typing, UsageGroups.Keyboard, UsageGroups.TrueMouse, UsageGroups.BlockPlayers);
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

        public static T InstantiateWindow<T>(in bool can_hide = false, in bool can_fullscreen = true, in bool can_cancel = true) where T : SguiWindow
        {
            T winwow = Util.InstantiateOrCreate<T>(SguiGlobal.instance.rT_2D);
            winwow.button_hide.interactable = can_hide;
            winwow.button_fullscreen.interactable = can_fullscreen;
            winwow.button_close.interactable = can_cancel;

            if (winwow is SguiCustom custom)
                custom.button_cancel.interactable = can_cancel;

            return winwow;
        }

        //--------------------------------------------------------------------------------------------------------------

        public void Oblivionize()
        {
            if (oblivionized)
                return;
            oblivionized = true;

            sgui_toggle_window.Update(false);
            OnOblivion();
        }

        protected virtual void OnOblivion()
        {
        }

        protected virtual void OnDestroy()
        {
            Oblivionize();
            UsageManager.RemoveUser(this);
        }
    }
}