using _ARK_;
using _UTIL_;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace _SGUI_
{
    public partial class SguiWindow : MonoBehaviour, SguiGlobal.ISguiGlobalLeftClick
    {
        public static readonly ListListener<SguiWindow> instances = new();

        public static readonly ListListener<SguiWindow> focused = new();

        public bool HasFocus() => this == focused.IsLast(this);

        [HideInInspector] public Animator animator;

        public Action<BaseStates, bool> onState, onState_once;

        public bool oblivionized;
        public Func<bool> onFunc_close;
        public Action onAction_close, onOblivion, onDestroy;

        [SerializeField] protected bool animate_hue = true;

        public Texture window_icon;
        protected SoftwareButton os_button;

        public readonly ValueNotifier<Traductions> sgui_description = new();

        static uint _id;
        public uint id;
        bool initialized;

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            _id = 0;
            instances.Reset();
            focused.Reset();
            focused.AddListener2(list => SoftwareButton.RefreshAllOpenStates());
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Awake() => Initialize();

        internal void Initialize()
        {
            if (initialized)
                return;

            initialized = true;
            OnAwake();
        }

        //--------------------------------------------------------------------------------------------------------------

        internal protected virtual void OnAwake()
        {
            id = _id++;

            if (TryGetComponent(out animator))
            {
                animator.writeDefaultValuesOnDisable = true;
                animator.keepAnimatorStateOnDisable = true;
            }

            AwakeUI();

            if (window_icon != null)
                os_button = OSView.instance.AddSoftwareButton(GetType(), new(GetType().FullName));

            trad_title.SetText($"[{id}] {GetType().Name}");
            sgui_description.Value = new($"[{id}] {GetType().FullName}");

            instances.AddElement(this);

            saved_size = rt.rect.size;
        }

        protected virtual void OnEnable()
        {
            NUCLEOR.delegates.LateUpdate -= UpdateHue;
            if (animate_hue)
                NUCLEOR.delegates.LateUpdate += UpdateHue;
            os_button?.RefreshOpenState();
        }

        protected virtual void OnDisable()
        {
            NUCLEOR.delegates.LateUpdate -= UpdateHue;
            os_button?.RefreshOpenState();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Start()
        {
            if (os_button != null)
                os_button.software_instances.AddElement(this);

            StartUI();
            ToggleWindow(true);
            animator.Update(0);

            button_close.onClick.AddListener(ResetScalePivot);

            focused.AddListener2(OnFocused);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static bool TryGetFocused<T>(out T output) where T : SguiWindow
        {
            if (focused.IsNotEmpty)
                if (focused._collection[^1] is T t)
                {
                    output = t;
                    return true;
                }
            output = null;
            return false;
        }

        public virtual void OnSguiGlobalLeftClick() => TakeFocus();
        public void TakeFocus()
        {
            if (oblivionized)
                return;

            focused.Modify(list =>
            {
                if (focused.IsLast(this))
                    return;
                list.Remove(this);
                list.Add(this);
                ToggleWindow(true);
            });
        }

        void OnFocused(List<SguiWindow> list) => OnFocus(focused.IsLast(this));
        protected virtual void OnFocus(in bool has_focus)
        {
            if (!has_focus)
                return;

#if UNITY_EDITOR
            SguiGlobal.instance._FOCUSED_WINDOW = this;
#endif

            transform.SetAsLastSibling();

            instances.Modify(list =>
            {
                list.Remove(this);
                list.Add(this);
            });
        }

        public void SetScalePivot(in SoftwareButton button)
        {
            if (button == null)
                rt_scale.pivot = .5f * Vector2.one;
            else
            {
                float x = RectTransformUtility.WorldToScreenPoint(null, os_button.rt.position).x;
                x /= Screen.width;
                rt_scale.pivot = new(x, 0);
            }
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

            NUCLEOR.delegates.LateUpdate -= UpdateHue;
            NUCLEOR.delegates.LateUpdate -= OnUpdateAlpha;
            ResizerVisual.instance?.UntakeFocus(this);

            button_close?.onClick.RemoveListener(ResetScalePivot);
            button_close?.onClick.RemoveListener(OnClickClose);

            onDestroy?.Invoke();
            UsageManager.RemoveUser(this);
            instances.RemoveElement(this);

            os_button?.RefreshOpenState();

            focused._listeners2 -= OnFocused;

            focused.RemoveElement(this);

            onState = onState_once = null;
            onFunc_close = null;
            onAction_close = onOblivion = onDestroy = null;
            onToggle = null;

            sgui_description.Reset();
            sgui_description.Dispose();
        }
    }
}
