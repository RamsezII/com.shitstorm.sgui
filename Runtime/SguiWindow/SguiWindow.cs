using _ARK_;
using _UTIL_;
using System;
using System.Collections.Generic;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;

namespace _SGUI_
{
    public partial class SguiWindow : ArkComponent1, SguiGlobal.ISguiGlobalLeftClick
    {
        [AutoStaticsCleanup] public static readonly ListListener<SguiWindow> instances = new();
        [AutoStaticsCleanup] public static readonly ListListener<SguiWindow> openWindows = new();
        public readonly ValueNotifier<bool> isFocused = new();

        [HideInInspector] public Animator animator;

        public bool oblivionized;
        public Func<bool> onFunc_close;
        public Action onAction_close, onOblivion;

        [SerializeField] protected bool animate_hue = true;

        public Texture window_icon;

        public Traductions sgui_description;

        [AutoStaticsCleanup] static uint _id;
        public uint id;
        bool initialized;

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
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
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            NUCLEOR.delegates.LateUpdate -= UpdateHue;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

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
                float localX = rt_scale.InverseTransformPoint(button.rt.position).x;
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

            openWindows._listeners2 -= OnWindowsListChanged;
            instances.RemoveElement(this);
            openWindows.RemoveElement(this);
            UsageManager.RemoveUser(this);
            ResizerVisual.instance?.UntakeFocus(this);

            button_close?.onClick.RemoveListener(ResetScalePivot);
            button_close?.onClick.RemoveListener(OnClickClose);
            SoftwareButton.RefreshAllOpenStates();

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
