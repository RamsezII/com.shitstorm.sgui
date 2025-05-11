using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    partial class SguiCustom
    {
        void AutoSize()
        {
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(SguiGlobal.instance.rT_2D);
            rT.ForceUpdateRectTransforms();

            float width = 350;
            float height = content_layout.preferredHeight;
            content_layout_rT.sizeDelta = new Vector2(0, height);

            for (int i = 0; i < clones.Count; i++)
            {
                SguiCustom_Abstract clone = clones[i];

                float pw = clone.tmp_label.preferredWidth;
                float cw = clone.rt_label.rect.width;
                float tw = clone.rt.rect.width;

                width = Mathf.Max(width, tw * ((25 + pw) / cw));
            }

            height += 60;
            rT.sizeDelta = new Vector2(width, height);
        }
    }
}