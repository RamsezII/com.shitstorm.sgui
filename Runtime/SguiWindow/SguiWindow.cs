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

        public Action<BaseStates, bool> onState, onState_once;
        public Action onDestroy;

        [SerializeField]
        protected bool
            animate_hue = true;

        public bool oblivionized;

        public readonly OnValue<bool> fullscreen = new();

        public SoftwareButton sgui_softwarebutton;

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

            sgui_softwarebutton?.instances.AddElement(this);
        }

        protected virtual void OnEnable()
        {
            NUCLEOR.delegates.onLateUpdate -= UpdateHue;
            if (animate_hue)
                NUCLEOR.delegates.onLateUpdate += UpdateHue;

            IMGUI_global.instance.users_inputs.AddElement(OnIMGui_toggle_fullscreen, this);
            OSView.instance.users.AddElement(this);
            UsageManager.AddUser(this, UsageGroups.TrueMouse, UsageGroups.Typing, UsageGroups.BlockPlayers, UsageGroups.Keyboard);
        }

        protected virtual void OnDisable()
        {
            NUCLEOR.delegates.onLateUpdate -= UpdateHue;
            IMGUI_global.instance.users_inputs.RemoveKey(OnIMGui_toggle_fullscreen);
            OSView.instance?.users.RemoveElement(this);
            UsageManager.RemoveUser(this);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Start()
        {
            StartUI();
            UsageManager.ToggleUser(this, true, UsageGroups.Typing, UsageGroups.Keyboard, UsageGroups.TrueMouse, UsageGroups.BlockPlayers);
            ToggleWindow(true);

            if (sgui_softwarebutton != null)
            {
                button_hide?.onClick.AddListener(() => SetScalePivot(sgui_softwarebutton));
                button_close?.onClick.AddListener(() => SetScalePivot(null));
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

        public void SetScalePivot(in SoftwareButton button)
        {
            if (button == null)
                rT_parent.pivot = .5f * Vector2.one;
            else
            {
                float x = RectTransformUtility.WorldToScreenPoint(null, sgui_softwarebutton.rt.position).x;
                x /= Screen.width;
                rT_parent.pivot = new(x, 0);
            }
        }

        public static T InstantiateWindow<T>(in bool can_hide = false, in bool can_fullscreen = true, in bool can_cancel = true) where T : SguiWindow => (T)InstantiateWindow(typeof(T), can_hide, can_fullscreen, can_cancel);
        public static SguiWindow InstantiateWindow(in Type type, in bool can_hide = false, in bool can_fullscreen = true, in bool can_cancel = true)
        {
            SguiWindow winwow = (SguiWindow)Util.InstantiateOrCreate(type, SguiGlobal.instance.rT_2D);
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

            ToggleWindow(false);
            instances.RemoveElement(this);

            if (sgui_softwarebutton != null)
                sgui_softwarebutton.instances.RemoveElement(this);

            OnOblivion();
        }

        protected virtual void OnOblivion()
        {
        }

        protected virtual void OnDestroy()
        {
            Oblivionize();
            onDestroy?.Invoke();
            UsageManager.RemoveUser(this);
            Debug.Log($"destroyed {GetType().FullName} ({transform.GetPath(true)})".ToSubLog());
        }
    }
}