using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public abstract partial class SguiWindow1 : SguiWindow
    {
        protected override void Awake()
        {
            huable_background = transform.Find("rT/body/background").GetComponent<Graphic>();

            buttons_rt = (RectTransform)transform.Find("rT/header/buttons");
            button_hide = buttons_rt.Find("hide/button").GetComponent<Button>();
            button_fullscreen = buttons_rt.Find("fullscreen/button").GetComponent<Button>();
            button_close = buttons_rt.Find("close/button").GetComponent<Button>();

            zone_header = transform.Find("rT/header/header_mask/padding/gradient").GetComponent<SguiZone>();
            zone_outline = transform.Find("rT/selected").GetComponent<SguiZone>();

            dropdown_settings = transform.Find("rT/buttons/layout/button_Settings")?.GetComponent<HeaderDropdown>();
            if (dropdown_settings != null)
                dropdown_settings.onItemClick += OnClickDropdown_Settings;

            dropdown_help = transform.Find("rT/buttons/layout/button_Help")?.GetComponent<HeaderDropdown>();
            if (dropdown_help != null)
                dropdown_help.onItemClick += OnClickDropdown_Help;

            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();
            OnPopulateDropdowns();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnApplicationFocus(bool focus)
        {
            if (focus)
                CheckBounds();
        }
    }
}