using _ARK_;
using _UTIL_;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace _SGUI_
{
    public partial class SguiWindow : ArkComponent1, SguiGlobal.ISguiGlobalLeftClick
    {
        public static readonly ListListener<SguiWindow> instances = new();
        public static readonly ListListener<SguiWindow> openWindows = new();
        public readonly ValueNotifier<bool> isFocused = new();

        [HideInInspector] public Animator animator;

        public bool oblivionized;
        public Func<bool> onFunc_close;
        public Action onAction_close, onOblivion;

        [SerializeField] protected bool animate_hue = true;

        public Texture window_icon;
        protected SoftwareButton os_button;

        public Traductions sgui_description;

        static uint _id;
        public uint id;
        bool initialized;

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            _id = 0;
            instances.Reset();
            openWindows.Reset();
            openWindows.AddListener2(list => SoftwareButton.RefreshAllOpenStates());
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            base.Awake();
            Initialize();
            InitToggle();
        }

        //--------------------------------------------------------------------------------------------------------------

        internal void Initialize()
        {
            if (initialized)
                return;

            initialized = true;
            OnInitialize();
        }

        internal protected virtual void OnInitialize()
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
            sgui_description = new($"[{id}] {GetType().FullName}");

            instances.AddElement(this);

            saved_size = rt.rect.size;

            openWindows.AddListener2(OnWindowsListChanged);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            NUCLEOR.delegates.LateUpdate -= UpdateHue;
            if (animate_hue)
                NUCLEOR.delegates.LateUpdate += UpdateHue;
            os_button?.RefreshOpenState();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            NUCLEOR.delegates.LateUpdate -= UpdateHue;
            os_button?.RefreshOpenState();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            if (os_button != null)
                os_button.software_instances.AddElement(this);

            StartUI();
            animator.Update(0);

            button_close.onClick.AddListener(ResetScalePivot);

            isFocused.AddListener(OnToggleFocus);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static bool TryGetFocused<T>(out T output) where T : SguiWindow
        {
            if (openWindows.IsNotEmpty)
                if (openWindows._collection[^1] is T t)
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

            openWindows.Modify(list =>
            {
                if (openWindows.IsLast(this))
                    return;
                list.Remove(this);
                list.Add(this);
                toggle.Value = true;
            });
        }

        void OnWindowsListChanged(List<SguiWindow> list) => isFocused.Value = list.Count > 0 && list[^1] == this;
        protected virtual void OnToggleFocus(bool focus)
        {
            if (!focus)
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
                float localX = rt_scale.InverseTransformPoint(os_button.rt.position).x;
                float x = Mathf.InverseLerp(rt_scale.rect.xMin, rt_scale.rect.xMax, localX);
                rt_scale.pivot = new(x, 0);
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        public void Oblivionize()
        {
            if (oblivionized)
                return;
            oblivionized = true;

            toggle.Value = false;
            instances.RemoveElement(this);
            openWindows.RemoveElement(this);

            if (os_button != null)
                os_button.software_instances.RemoveElement(this);

            openWindows._listeners2 -= OnWindowsListChanged;
            instances.RemoveElement(this);
            openWindows.RemoveElement(this);
            UsageManager.RemoveUser(this);
            ResizerVisual.instance?.UntakeFocus(this);

            button_close?.onClick.RemoveListener(ResetScalePivot);
            button_close?.onClick.RemoveListener(OnClickClose);
            os_button?.RefreshOpenState();

            onFunc_close = null;
            onAction_close = null;

            OnOblivion();
            onOblivion?.Invoke();
        }

        protected virtual void OnOblivion()
        {
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            Oblivionize();

            NUCLEOR.delegates.LateUpdate -= UpdateHue;
            NUCLEOR.delegates.LateUpdate -= OnUpdateAlpha;
        }
    }
}
