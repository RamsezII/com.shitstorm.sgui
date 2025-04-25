using _UTIL_;
using UnityEngine.UI;

namespace _SGUI_
{
    partial class SguiWindow1
    {
        internal HeaderDropdown dropdown_help;

        //--------------------------------------------------------------------------------------------------------------

        void AwakeUI()
        {
            trad_title = transform.Find("rT/header/title").GetComponent<Traductable>();
            body_background = transform.Find("rT/body/background").GetComponent<Graphic>();

            button_hide = transform.Find("rT/header/buttons/hide/button").GetComponent<Button>();
            button_fullscreen = transform.Find("rT/header/buttons/fullscreen/button").GetComponent<Button>();
            button_close = transform.Find("rT/header/buttons/close/button").GetComponent<Button>();

            zone_header = transform.Find("rT/header/header_mask/padding/gradient").GetComponent<SguiZone>();
            zone_outline = transform.Find("rT/selected").GetComponent<SguiZone>();

            dropdown_help = transform.Find("rT/buttons/layout/button_Help")?.GetComponent<HeaderDropdown>();

            if (dropdown_help != null)
                dropdown_help.onItemClick += OnClickDropdown_Help;
        }
    }
}