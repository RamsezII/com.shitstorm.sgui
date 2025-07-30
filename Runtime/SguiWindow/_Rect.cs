using UnityEngine;

namespace _SGUI_
{
    partial class SguiWindow
    {
        internal readonly struct SguiRect
        {
            public readonly Vector2 pivot_and_anchor, position, size;

            //--------------------------------------------------------------------------------------------------------------

            public SguiRect(in RectTransform rT)
            {
                pivot_and_anchor = rT.pivot;
                position = rT.anchoredPosition;
                size = rT.sizeDelta;
            }

            //--------------------------------------------------------------------------------------------------------------

            public readonly void Apply(in RectTransform rT)
            {
                rT.pivot = rT.anchorMin = rT.anchorMax = pivot_and_anchor;
                rT.anchoredPosition = position;
                rT.sizeDelta = size;
            }
        }
    }
}