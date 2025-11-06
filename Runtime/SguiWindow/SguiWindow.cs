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

        public bool oblivionized;
        public Func<bool> onFunc_close;
        public Action onAction_close, onOblivion, onDestroy;

        [SerializeField] protected bool animate_hue = true;

        public readonly ValueHandler<bool> fullscreen = new();

        public Texture window_icon;
        protected SoftwareButton os_button;

        static uint _id;
        public uint id = _id++;

        public override string ToString() => $"{GetType()}[{id}] \"{trad_title.traductions.Automatic}\"";

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnBeforeSceneLoad()
        {
            _id = 0;
        }

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

            if (window_icon != null)
            {
                os_button = OSView.instance.AddOrGetSoftwareButton(GetType());
                os_button.software_instances.AddElement(this);
            }
        }

        protected virtual void OnEnable()
        {
            NUCLEOR.delegates.LateUpdate -= UpdateHue;
            if (animate_hue)
                NUCLEOR.delegates.LateUpdate += UpdateHue;

            IMGUI_global.instance.users_inputs.AddElement(OnIMGui_toggle_fullscreen, this);
            OSView.instance.users.AddElement(this);
            UsageManager.AddUser(this, UsageGroups.GameMouse, UsageGroups.Typing, UsageGroups.BlockPlayer, UsageGroups.Keyboard);
        }

        protected virtual void OnDisable()
        {
            NUCLEOR.delegates.LateUpdate -= UpdateHue;
            IMGUI_global.instance.users_inputs.RemoveKey(OnIMGui_toggle_fullscreen);
            OSView.instance?.users.RemoveElement(this);
            UsageManager.RemoveUser(this);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Start()
        {
            StartUI();
            ToggleWindow(true);

            button_hide?.onClick.AddListener(() => SetScalePivot(os_button));
            button_close?.onClick.AddListener(() => SetScalePivot(null));
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
                float x = RectTransformUtility.WorldToScreenPoint(null, os_button.rt.position).x;
                x /= Screen.width;
                rT_parent.pivot = new(x, 0);
            }
        }

        public static T InstantiateWindow<T>(in bool can_hide = false, in bool can_fullscreen = true, in bool can_close = true) where T : SguiWindow => (T)InstantiateWindow(typeof(T), can_hide, can_fullscreen, can_close);
        public static SguiWindow InstantiateWindow(in Type type, in bool can_hide = false, in bool can_fullscreen = true, in bool can_close = true) => InstantiateWindow((SguiWindow)Util.LoadResourceByType(type), can_hide, can_fullscreen, can_close);
        public static SguiWindow InstantiateWindow(in SguiWindow prefab, in bool can_hide = false, in bool can_fullscreen = true, in bool can_close = true)
        {
            SguiWindow winwow = Instantiate(prefab, SguiGlobal.instance.rt_windows);

            winwow.button_hide.interactable = can_hide;
            winwow.button_fullscreen.interactable = can_fullscreen;
            winwow.button_close.interactable = can_close; // if is sgui_window2

            if (winwow is SguiCustom custom)
                custom.button_cancel.interactable = can_close;

            return winwow;
        }

        public static SguiCustom ShowAlert(in SguiDialogTypes type, out SguiCustom_Alert alert, in Traductions traductions)
        {
            SguiCustom sgui = InstantiateWindow<SguiCustom>();

            switch (type)
            {
                case SguiDialogTypes.Info:
                    Debug.Log($"{sgui.GetType()}.{type}: \"{traductions.Automatic}\"", sgui);
                    break;

                case SguiDialogTypes.Dialog:
                    Debug.Log($"{sgui.GetType()}.{type}: \"{traductions.Automatic}\"", sgui);
                    break;

                case SguiDialogTypes.Error:
                    Debug.LogWarning($"{sgui.GetType()}.{type}: \"{traductions.Automatic}\"", sgui);
                    break;

                default:
                    Debug.LogError($"{sgui.GetType()}.{type}: \"{traductions.Automatic}\"", sgui);
                    break;
            }

            alert = sgui.AddButton<SguiCustom_Alert>();
            alert.SetType(type);
            alert.SetText(traductions);
            sgui.trad_title.SetTrad(type.ToString());

            if (type != SguiDialogTypes.Dialog)
            {
                sgui.button_cancel.gameObject.SetActive(false);
                sgui.trad_confirm.SetTrad("OK");
            }

            return sgui;
        }

        //--------------------------------------------------------------------------------------------------------------

        public void Oblivionize()
        {
            if (oblivionized)
                return;
            oblivionized = true;

            ToggleWindow(false);
            instances.RemoveElement(this);

            if (os_button != null)
                os_button.software_instances.RemoveElement(this);

            OnOblivion();
            onOblivion?.Invoke();
        }

        protected virtual void OnOblivion()
        {
        }

        protected virtual void OnDestroy()
        {
            Oblivionize();
            onDestroy?.Invoke();
            UsageManager.RemoveUser(this);
        }
    }
}