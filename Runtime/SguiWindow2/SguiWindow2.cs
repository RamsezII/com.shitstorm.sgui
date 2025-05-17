using _UTIL_;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public class SguiWindow2 : SguiWindow
    {
        protected override void Awake()
        {
            base.Awake();

            trad_title = transform.Find("rT/header/title").GetComponent<Traductable>();
            huable_background = transform.Find("rT/background").GetComponent<Graphic>();

            buttons_rt = (RectTransform)transform.Find("rT/header/buttons");
            button_hide = buttons_rt.Find("button-hide/Button").GetComponent<Button>();
            button_fullscreen = buttons_rt.Find("button-fullscreen/Button").GetComponent<Button>();
            button_close = buttons_rt.Find("button-close/Button").GetComponent<Button>();

            zone_header = transform.Find("rT/header/drag_zone").GetComponent<SguiZone>();
            zone_outline = transform.Find("rT/zone_size").GetComponent<SguiZone>();
        }
    }
}