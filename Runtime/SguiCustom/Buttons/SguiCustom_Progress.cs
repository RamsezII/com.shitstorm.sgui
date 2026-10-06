using TMPro;
using UnityEngine;

namespace _SGUI_
{
    public class SguiCustom_Progress : SguiCustom_Abstract
    {
        public RectTransform rT_fill;
        public TextMeshProUGUI tmp_percentage;

        //--------------------------------------------------------------------------------------------------------------

        public void SetProgress(in float progress)
        {
            rT_fill.anchorMax = new(progress, 1);
            tmp_percentage.text = $"{Mathf.RoundToInt(progress * 100)}%";
        }
    }
}