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
                os_button = OSView.instance.GetSoftwareButton(GetType(), force: true);
                os_button.software_instances.AddElement(this);
            }

            trad_title.SetTrad(GetType().Name);
        }

        protected virtual void OnEnable()
        {
            NUCLEOR.delegates.LateUpdate -= UpdateHue;
            if (animate_hue)
                NUCLEOR.delegates.LateUpdate += UpdateHue;

            OSView.instance.users.AddElement(this);
            UsageManager.AddUser(this, UsageGroups.TrueMouse, UsageGroups.Typing, UsageGroups.BlockPlayer, UsageGroups.Keyboard);
        }

        protected virtual void OnDisable()
        {
            NUCLEOR.delegates.LateUpdate -= UpdateHue;
            OSView.instance?.users.RemoveElement(this);
            UsageManager.RemoveUser(this);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void Start()
        {
            StartUI();
            ToggleWindow(true);
            button_close.onClick.AddListener(() => SetScalePivot(null));
        }

        //--------------------------------------------------------------------------------------------------------------

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

        public static T InstantiateWindow<T>() where T : SguiWindow => (T)InstantiateWindow(typeof(T));
        public static SguiWindow InstantiateWindow(in Type type) => InstantiateWindow((SguiWindow)Util.LoadResourceByType(type));
        public static SguiWindow InstantiateWindow(in SguiWindow prefab)
        {
            RectTransform parent_rt = prefab is SguiWindow1
                ? SguiGlobal.instance.rt_windows1
                : SguiGlobal.instance.rt_windows2;

            SguiWindow winwow = Instantiate(prefab, parent_rt);

            return winwow;
        }

        public static SguiCustom ShowAlert(in SguiDialogs type, out SguiCustom_Alert alert, in Traductions traductions)
        {
            SguiCustom sgui = InstantiateWindow<SguiCustom>();

            switch (type)
            {
                case SguiDialogs.Info:
                    Debug.Log($"{sgui.GetType()}.{type}: \"{traductions.Automatic}\"", sgui);
                    break;

                case SguiDialogs.Dialog:
                    Debug.Log($"{sgui.GetType()}.{type}: \"{traductions.Automatic}\"", sgui);
                    break;

                case SguiDialogs.Error:
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

            var tmpro_title = window.trad_title.FirstTmp();
            tmpro_title.alignment = TMPro.TextAlignmentOptions.MidlineLeft;

            return window;
        }

        public static SguiCustom_Progress InstantiateProgressBar_OLD(in bool no_label)
        {
            SguiCustom window = InstantiateWindow<SguiCustom>();

            SguiCustom_Progress progress = window.AddButton<SguiCustom_Progress>();

            window.button_confirm.transform.gameObject.SetActive(false);
            window.button_close.transform.parent.parent.gameObject.SetActive(false);

            var tmpro_title = window.trad_title.FirstTmp();
            tmpro_title.alignment = TMPro.TextAlignmentOptions.MidlineLeft;

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
        }
    }
}