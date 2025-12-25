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
        public void TakeFocus() => focused.Modify(list =>
        {
            if (focused.IsLast(this))
                return;
            list.Remove(this);
            list.Add(this);
            ToggleWindow(true);
        });
        public bool HasFocus() => this == focused.IsLast(this);

        [HideInInspector] public Animator animator;

        public Action<BaseStates, bool> onState, onState_once;

        public bool oblivionized;
        public Func<bool> onFunc_close;
        public Action onAction_close, onOblivion, onDestroy;

        [SerializeField] protected bool animate_hue = true;

        public Texture window_icon;
        protected SoftwareButton os_button;

        public readonly ValueHandler<Traductions> sgui_description = new();

        static uint _id;
        public uint id;

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

        protected virtual void OnAwake()
        {
            id = _id++;

            if (TryGetComponent(out animator))
            {
                animator.writeDefaultValuesOnDisable = true;
                animator.keepAnimatorStateOnDisable = true;
                animator.Update(0);
            }
            AwakeUI();

            if (window_icon != null)
                os_button = OSView.instance.AddSoftwareButton(GetType(), new(GetType().FullName));

            trad_title.SetTrad($"[{id}] {GetType().Name}");
            sgui_description.Value = new($"[{id}] {GetType().FullName}");

            instances.AddElement(this);
        }

        protected virtual void OnEnable()
        {
            NUCLEOR.delegates.LateUpdate -= UpdateHue;
            if (animate_hue)
                NUCLEOR.delegates.LateUpdate += UpdateHue;

            UsageManager.AddUser(this, UsageGroups.TrueMouse, UsageGroups.BlockPlayer, UsageGroups.Keyboard);

            os_button?.RefreshOpenState();
        }

        protected virtual void OnDisable()
        {
            NUCLEOR.delegates.LateUpdate -= UpdateHue;
            UsageManager.RemoveUser(this);

            os_button?.RefreshOpenState();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Start()
        {
            if (os_button != null)
                os_button.software_instances.AddElement(this);

            StartUI();
            ToggleWindow(true);
            button_close.onClick.AddListener(() => SetScalePivot(null));

            focused.AddListener2(OnFocused);
        }

        //--------------------------------------------------------------------------------------------------------------

        public virtual void OnSguiGlobalLeftClick()
        {
            TakeFocus();
        }

        void OnFocused(List<SguiWindow> list) => OnFocus(focused.IsLast(this));
        protected virtual void OnFocus(in bool has_focus)
        {
            if (!has_focus)
                return;

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
                rt_parent.pivot = .5f * Vector2.one;
            else
            {
                float x = RectTransformUtility.WorldToScreenPoint(null, os_button.rt.position).x;
                x /= Screen.width;
                rt_parent.pivot = new(x, 0);
            }
        }

        public static T InstantiateWindow<T>(in bool as_software = false) where T : SguiWindow => (T)InstantiateWindow(typeof(T), as_software: false);
        public static SguiWindow InstantiateWindow(in Type type, in bool as_software = false) => InstantiateWindow((SguiWindow)Util.LoadResourceByType(type), as_software: as_software);
        public static SguiWindow InstantiateWindow(in SguiWindow prefab, in bool as_software = false)
        {
            RectTransform parent_rt = as_software || prefab is SguiWindow1
                ? OSView.instance.windows_rt
                : SguiGlobal.instance.rt_windows2;

            SguiWindow clone = Instantiate(prefab, parent_rt);
            clone.OnAwake();

            if (prefab is SguiWindow1)
                OSView.instance.ToggleSelf(true);

            return clone;
        }

        public static SguiCustom ShowAlert(in SguiDialogs type, out SguiCustom_Alert alert, in Traductions traductions)
        {
            SguiCustom sgui = InstantiateWindow<SguiCustom>();

            switch (type)
            {
                case SguiDialogs.Info:
                    Debug.Log($"{sgui.GetType()}.{type}: \"{traductions.GetAutomatic()}\"", sgui);
                    break;

                case SguiDialogs.Dialog:
                    Debug.Log($"{sgui.GetType()}.{type}: \"{traductions.GetAutomatic()}\"", sgui);
                    break;

                case SguiDialogs.Error:
                    Debug.LogWarning($"{sgui.GetType()}.{type}: \"{traductions.GetAutomatic()}\"", sgui);
                    break;

                default:
                    Debug.LogError($"{sgui.GetType()}.{type}: \"{traductions.GetAutomatic()}\"", sgui);
                    break;
            }

            alert = sgui.AddButton<SguiCustom_Alert>();
            alert.SetType(type);
            alert.SetText(traductions);
            sgui.trad_title.SetTrad(type.ToString());

            if (type == SguiDialogs.Dialog)
            {
                sgui.trad_confirm.SetTrads(new() { french = "Oui", english = "Yes", });
                sgui.trad_cancel.SetTrads(new() { french = "Non", english = "No", });
            }
            else
            {
                sgui.button_cancel.gameObject.SetActive(false);
                sgui.trad_confirm.SetTrad("OK");
            }

            return sgui;
        }

        public static SguiProgressBar InstantiateProgressBar_NEW()
        {
            SguiProgressBar window = InstantiateWindow<SguiProgressBar>();
            window.trad_title.tmpro.alignment = TMPro.TextAlignmentOptions.MidlineLeft;
            return window;
        }

        public static SguiCustom_Progress InstantiateProgressBar_OLD(in bool no_label)
        {
            SguiCustom window = InstantiateWindow<SguiCustom>();

            SguiCustom_Progress progress = window.AddButton<SguiCustom_Progress>();

            window.button_confirm.transform.gameObject.SetActive(false);
            window.button_close.transform.parent.parent.gameObject.SetActive(false);
            window.trad_title.tmpro.alignment = TMPro.TextAlignmentOptions.MidlineLeft;

            if (no_label)
            {
                progress.rT_label.gameObject.SetActive(false);
                RectTransform rt = (RectTransform)progress.rT_fill.parent.parent;
                rt.anchorMin = new(0, .5f);
            }

            return progress;
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
            instances.RemoveElement(this);

            os_button?.RefreshOpenState();

            focused._listeners2 -= OnFocused;

            focused.RemoveElement(this);
        }
    }
}