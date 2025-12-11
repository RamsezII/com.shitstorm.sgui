using _ARK_;
using _UTIL_;
using TMPro;
using UnityEngine;

namespace _SGUI_
{
    public class SguiMonitor_Resources_Section : SguiMonitor_Resources_Addable
    {
        RectTransform arrow_rt;
        public Traductable trad_label;
        public TextMeshProUGUI text_label;
        public PointerClickHandler click_handler;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            arrow_rt = (RectTransform)transform.Find("arrow");
            text_label = GetComponentInChildren<TextMeshProUGUI>();
            trad_label = GetComponentInChildren<Traductable>();
            base.Awake();
        }
    }
}