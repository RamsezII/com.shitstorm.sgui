using _UTIL_;
using UnityEngine.UI;

namespace _SGUI_
{
    public class SguiWindow2 : SguiWindow
    {
        protected override void Awake()
        {
            base.Awake();

            trad_title = transform.Find("rT/header/text").GetComponent<Traductable>();
            huable_background = transform.Find("rT/background").GetComponent<Graphic>();

            button_hide = transform.Find("rT/header/buttons/button-hide/Button").GetComponent<Button>();
            button_fullscreen = transform.Find("rT/header/buttons/button-fullscreen/Button").GetComponent<Button>();
            button_close = transform.Find("rT/header/buttons/button-close/Button").GetComponent<Button>();

            zone_header = transform.Find("rT/header/drag_zone").GetComponent<SguiZone>();
            zone_outline = transform.Find("rT/zone_size").GetComponent<SguiZone>();
        }
    }
}