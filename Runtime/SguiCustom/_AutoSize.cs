using _ARK_;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    partial class SguiCustom
    {
        public float autosize_offset_height = 60;

        //--------------------------------------------------------------------------------------------------------------

        public void AutoSizeAtEndOfFrame() => Util.AddAction(ref NUCLEOR.delegates.LateUpdate_onEndOfFrame_once, AutoSizeNow);
        public void AutoSizeNow()
        {
            if (this == null)
                return;

            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate(content_layout_rT);
            rt.ForceUpdateRectTransforms();

            float width = 350;
            float height = content_layout.preferredHeight;
            content_layout_rT.sizeDelta = new Vector2(0, height);

            SguiCustom_Abstract[] clones = GetComponentsInChildren<SguiCustom_Abstract>(false);

            for (int i = 0; i < clones.Length; i++)
            {
                SguiCustom_Abstract clone = clones[i];

                float pw = clone.tmp_label.preferredWidth;
                float cw = clone.rT_label.rect.width;
                float tw = clone.rT.rect.width;

                width = Mathf.Max(width, tw * ((25 + pw) / cw));
            }

            height += autosize_offset_height;
            rt.sizeDelta = new Vector2(width, height);
        }
    }
}