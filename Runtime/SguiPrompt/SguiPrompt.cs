using _ARK_;
using _UTIL_;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public class SguiPrompt : SguiWindow
    {
        CanvasGroup canvasGroup_rt;
        public RawImage rimg_background;
        public Button button_confirm, button_cancel;
        public Traductable trad_cancel, trad_confirm;

        public Func<bool> onFunc_confirm;
        public Action onAction_confirm;

        //--------------------------------------------------------------------------------------------------------------

        internal protected override void OnInitialize()
        {
            base.OnInitialize();

            canvasGroup_rt = rt.GetComponent<CanvasGroup>();

            rimg_background = transform.Find("background").GetComponent<RawImage>();
            huable_background = rt.Find("background").GetComponent<Graphic>();

            buttons_rt = (RectTransform)rt.Find("header/buttons");
            button_close = buttons_rt.Find("button-close/Button").GetComponent<Button>();

            button_cancel = rt.Find("footer/button_cancel").GetComponent<Button>();
            button_confirm = rt.Find("footer/button_confirm").GetComponent<Button>();

            trad_cancel = button_cancel.transform.Find("label").GetComponent<Traductable>();
            trad_confirm = button_confirm.transform.Find("label").GetComponent<Traductable>();

            button_confirm.onClick.AddListener(() =>
            {
                if (!oblivionized)
                    if (onFunc_confirm != null && !onFunc_confirm())
                        return;
                onAction_confirm?.Invoke();
                Oblivionize();
            });

            button_cancel.onClick.AddListener(OnClickClose);

            DragHandler drag_handler = rt.Find("header/drag_zone").GetComponent<DragHandler>();
            drag_handler.onBeginDrag += OnHeaderBeginDrag;
            drag_handler.onDrag += OnHeaderDrag;
            drag_handler.onEndDrag += OnHeaderEndDrag;

            UsageManager.AddUser(this, UsageGroups.BlockPlayer, UsageGroups.TrueMouse, UsageGroups.Keyboard);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnOblivion()
        {
            base.OnOblivion();
            canvasGroup_rt.interactable = false;
            UsageManager.RemoveUser(this);
        }
    }
}