using System;
using UnityEngine;

namespace _SGUI_
{
    partial class SguiWindow1
    {
        enum DragModes : byte
        {
            _none_,
            top,
            right,
            bottom,
            left,
        }

        [Flags]
        public enum DRAG_MODES : byte
        {
            _none_,
            TOP = 1 << DragModes.top,
            RIGHT = 1 << DragModes.right,
            BOTTOM = 1 << DragModes.bottom,
            LEFT = 1 << DragModes.left,
        }

        readonly Vector2 minimum_size = new(200, 150);
        [SerializeField] DRAG_MODES dragmode;

        //--------------------------------------------------------------------------------------------------------------

        public Vector2 ScreenPositionToLocal(Vector2 screen_pos)
        {
            Vector2 size_parent = rT_parent.rect.size;
            Vector2 pos = rT.anchoredPosition;

            screen_pos.x /= Screen.width;
            screen_pos.y /= Screen.height;

            screen_pos.x *= size_parent.x;
            screen_pos.y *= size_parent.y;

            screen_pos -= pos;

            return screen_pos;
        }

        public Vector2 ScreenDeltaToLocal(Vector2 screen_delta)
        {
            Vector2 size_parent = rT_parent.rect.size;

            screen_delta.x /= Screen.width;
            screen_delta.y /= Screen.height;

            screen_delta.x *= size_parent.x;
            screen_delta.y *= size_parent.y;

            return screen_delta;
        }

        void OnHeaderDrag(Vector2 delta)
        {
            rT.anchoredPosition += ScreenDeltaToLocal(delta);
            CheckBounds();
        }

        public void CheckBounds()
        {
            Vector2 parent_size = rT_parent.rect.size;
            Vector2 size = rT.rect.size;
            Vector2 pos = rT.anchoredPosition;

            Vector2 corner_sw = pos;
            Vector2 corner_ne = pos + size;

            corner_sw.x = Mathf.Clamp(corner_sw.x, 0, parent_size.x - minimum_size.x);
            corner_sw.y = Mathf.Clamp(corner_sw.y, 0, parent_size.y - minimum_size.y);

            corner_ne.x = Mathf.Clamp(corner_ne.x, minimum_size.x, parent_size.x);
            corner_ne.y = Mathf.Clamp(corner_ne.y, minimum_size.y, parent_size.y);

            rT.anchoredPosition = corner_sw;
            rT.sizeDelta = corner_ne - corner_sw;

            OnCheckBounds();
        }

        protected virtual void OnCheckBounds()
        {
        }

        void OnSizeDrag_begin(Vector2 mouse_pos)
        {
            Vector2 size = rT.rect.size;

            mouse_pos = ScreenPositionToLocal(mouse_pos);

            mouse_pos.x /= size.x;
            mouse_pos.y /= size.y;

            dragmode = 0;

            if (mouse_pos.x >= .9f)
                dragmode |= DRAG_MODES.RIGHT;
            if (mouse_pos.x <= .1f)
                dragmode |= DRAG_MODES.LEFT;
            if (mouse_pos.y >= .9f)
                dragmode |= DRAG_MODES.TOP;
            if (mouse_pos.y <= .1f)
                dragmode |= DRAG_MODES.BOTTOM;
        }

        void OnSizeDrag(Vector2 delta)
        {
            delta = ScreenDeltaToLocal(delta);

            if (dragmode.HasFlag(DRAG_MODES.TOP))
                rT.sizeDelta += delta * Vector2.up;

            if (dragmode.HasFlag(DRAG_MODES.RIGHT))
                rT.sizeDelta += delta * Vector2.right;

            if (dragmode.HasFlag(DRAG_MODES.BOTTOM))
            {
                rT.anchoredPosition += delta * Vector2.up;
                rT.sizeDelta += delta * Vector2.down;
            }

            if (dragmode.HasFlag(DRAG_MODES.LEFT))
            {
                rT.anchoredPosition += delta * Vector2.right;
                rT.sizeDelta += delta * Vector2.left;
            }

            CheckBounds();
        }
    }
}