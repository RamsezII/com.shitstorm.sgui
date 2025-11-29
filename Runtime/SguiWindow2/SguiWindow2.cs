using _ARK_;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public class SguiWindow2 : SguiWindow
    {
        public Button button_confirm, button_cancel;
        public Traductable trad_cancel, trad_confirm;

        public Func<bool> onFunc_confirm;
        public Action onAction_confirm;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            huable_background = transform.Find("rT/background").GetComponent<Graphic>();

            buttons_rt = (RectTransform)transform.Find("rT/header/buttons");
            button_hide = buttons_rt.Find("button-hide/Button").GetComponent<Button>();
            button_fullscreen = buttons_rt.Find("button-fullscreen/Button").GetComponent<Button>();
            button_close = buttons_rt.Find("button-close/Button").GetComponent<Button>();

            zone_header = transform.Find("rT/header/drag_zone").GetComponent<SguiZone>();
            zone_outline = transform.Find("rT/zone_size").GetComponent<SguiZone>();

            button_cancel = transform.Find("rT/footer/button_cancel").GetComponent<Button>();
            button_confirm = transform.Find("rT/footer/button_confirm").GetComponent<Button>();

            trad_cancel = button_cancel.transform.Find("label").GetComponent<Traductable>();
            trad_confirm = button_confirm.transform.Find("label").GetComponent<Traductable>();

            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            button_confirm.onClick.AddListener(() =>
            {
                if (!oblivionized)
                    if (onFunc_confirm != null && !onFunc_confirm())
                        return;
                onAction_confirm?.Invoke();
                Oblivionize();
            });

            button_cancel.onClick.AddListener(OnClickClose);
        }
    }
}