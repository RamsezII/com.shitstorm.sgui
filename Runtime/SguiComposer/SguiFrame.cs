using _ARK_;
using _UTIL_;
using System;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_.composer
{
    public abstract partial class SguiFrame : ArkComponent1, SguiGlobal.ISguiGlobalLeftClick
    {
        [AutoStaticsCleanup] public static readonly ListListener<SguiFrame> instances = new();
        public readonly ValueNotifier<bool> isFocused = new();
        internal SguiSplitView pview;
        internal SguiTabHeader tab;
        public SguiComposer composer => pview == null ? null : pview.composer;
        public RectTransform rt;
        public Canvas canvas;
        public GraphicRaycaster raycaster;
        public Texture window_icon;
        public Traductions sgui_name, sgui_description;
        public bool oblivionized;
        public Func<bool> onFunc_close;
        public Action onAction_close, onOblivion;
        public SoftwareButton os_button;

        protected override void Awake()
        {
            pview = GetComponentInParent<SguiSplitView>(true);
            pview = GetComponentInParent<SguiSplitView>(true);
            canvas = GetComponentInParent<Canvas>(true);
            raycaster = GetComponentInParent<GraphicRaycaster>(true);

            rt = (RectTransform)transform;

            base.Awake();

            instances.AddElement(this);

            if (OSView.instance != null)
            {
                os_button = OSView.instance.AddSoftwareButton(GetType(), sgui_description);
                os_button.users.AddElement(this);
            }
        }

        protected override void Start()
        {
            base.Start();
            isFocused.AddListener(OnToggleFocus);
            OnResized();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            RefreshFocus();
        }

        protected override void OnDisable()
        {
            isFocused.Value = false;
            base.OnDisable();
        }

        internal void RefreshFocus()
        {
            isFocused.Value = !oblivionized && isActiveAndEnabled && composer != null && composer.toggle._value && composer.isFocused._value && composer.active_frame._value == this;
            SoftwareButton.RefreshAllOpenStates();
        }

        protected virtual void OnToggleFocus(bool focus) { }
        public virtual void OnResized() { }
        public void OnSguiGlobalLeftClick() => TakeFocus();

        public void TakeFocus()
        {
            if (oblivionized || composer == null) return;
            composer.CancelCloseRequest();
            OSView.instance.ToggleSelf(true);
            pview.SelectFrame(this);
            composer.TakeFocus();
            RefreshFocus();
        }

        public static bool TryGetFocused<T>(out T output) where T : SguiFrame
        {
            if (SguiWindow.openWindows.LastOrDefault is SguiComposer owner && owner.active_frame._value is T frame && frame.isFocused._value)
            {
                output = frame;
                return true;
            }
            output = null;
            return false;
        }

        public bool RequestClose()
        {
            if (oblivionized) return true;
            if (onFunc_close != null && !onFunc_close()) return false;
            onAction_close?.Invoke();
            Oblivionize();
            return true;
        }

        public void Oblivionize()
        {
            if (oblivionized) return;
            oblivionized = true;
            isFocused.Value = false;
            instances.RemoveElement(this);
            os_button?.users.RemoveElement(this);
            UsageManager.RemoveUser(this);
            pview?.RemoveFrame(this);
            OnOblivion();
            onOblivion?.Invoke();
            onFunc_close = null;
            onAction_close = onOblivion = null;
            if (!_destroyed) Destroy(gameObject);
        }

        protected virtual void OnOblivion() { }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Oblivionize();
            isFocused.Clear();
        }

        public static SguiCustom CreatePrompt() => SguiWindow.CreatePrompt();
        public static SguiCustom ShowAlert(in SguiDialogs type, out SguiCustom_Alert alert, in Traductions traductions) => SguiWindow.ShowAlert(type, out alert, traductions);
        public static SguiCustom ShowProgressBar(out SguiCustom_Progress progress_bar, in bool no_label = false, in bool no_cancel = false) => SguiWindow.ShowProgressBar(out progress_bar, no_label, no_cancel);
    }
}
