using UnityEngine;

namespace _SGUI_
{
    public class SguiCustom_Progress : SguiCustom_Abstract
    {
        public RectTransform rT_fill;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            rT_fill = (RectTransform)transform.Find("progress-bar/fill");
            base.Awake();
        }
    }
}