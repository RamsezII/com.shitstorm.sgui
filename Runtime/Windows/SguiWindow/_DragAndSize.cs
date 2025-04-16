using UnityEngine;

namespace _SGUI_
{
    partial class SguiWindow
    {
        readonly Vector2 minimum_size = new(200, 150);

        //--------------------------------------------------------------------------------------------------------------

        void OnHeaderDrag(Vector2 delta)
        {
            Vector2 screen_size = rT_parent.rect.size;
            Vector2 this_size = rT.rect.size;

            Vector2 old_pos = rT.anchoredPosition;
            Vector2 current_pos = old_pos + delta;
        }

        void OnOutlineBeginDrag(Vector2 position)
        {

        }

        void OnOutlineDrag(Vector2 delta)
        {

        }
    }
}