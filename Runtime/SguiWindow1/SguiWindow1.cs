using _UTIL_;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public abstract partial class SguiWindow1 : SguiWindow
    {
        internal HeaderDropdown dropdown_help;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            base.Awake();

            trad_title = transform.Find("rT/header/title").GetComponent<Traductable>();
            huable_background = transform.Find("rT/body/background").GetComponent<Graphic>();

            buttons_rt = (RectTransform)transform.Find("rT/header/buttons");
            button_hide = buttons_rt.Find("hide/button").GetComponent<Button>();
            button_fullscreen = buttons_rt.Find("fullscreen/button").GetComponent<Button>();
            button_close = buttons_rt.Find("close/button").GetComponent<Button>();

            zone_header = transform.Find("rT/header/header_mask/padding/gradient").GetComponent<SguiZone>();
            zone_outline = transform.Find("rT/selected").GetComponent<SguiZone>();

            dropdown_help = transform.Find("rT/buttons/layout/button_Help")?.GetComponent<HeaderDropdown>();

            if (dropdown_help != null)
                dropdown_help.onItemClick += OnClickDropdown_Help;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();
            OnPopulateDropdowns();

            button_hide?.onClick.AddListener(() => SetScalePivot(SguiGlobal.instance.button_terminal));
            button_close?.onClick.AddListener(() => SetScalePivot(null));
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnApplicationFocus(bool focus)
        {
            if (focus)
                CheckBounds();
        }

        public void SetScalePivot(in SoftwareButton button)
        {
            if (button == null)
                rT_parent.pivot = .5f * Vector2.one;
            else
            {
                float x = RectTransformUtility.WorldToScreenPoint(null, SguiGlobal.instance.button_terminal.rt.position).x;
                x /= Screen.width;
                rT_parent.pivot = new(x, 0);
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnOblivion()
        {
            base.OnOblivion();
            instances.RemoveElement(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onDestroy?.Invoke();
            Debug.Log($"destroyed {GetType().FullName} ({transform.GetPath(true)})".ToSubLog());
        }
    }
}