using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    partial class SguiWindow
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

            TOP_RIGHT = TOP | RIGHT,
            BOTTOM_RIGHT = BOTTOM | RIGHT,
            BOTTOM_LEFT = BOTTOM | LEFT,
            TOP_LEFT = TOP | LEFT,

            ALL = TOP | RIGHT | BOTTOM | LEFT,
        }

        readonly Vector2 minimum_size = new(200, 150);
        [SerializeField] DRAG_MODES drag_mode;
        [SerializeField] CursorManager_OLD.Cursors cursor_mode;

        //--------------------------------------------------------------------------------------------------------------

        internal void OnZoneEvent(in SguiZone.Codes code, in SguiZone zone, in PointerEventData data)
        {
            switch (code)
            {
                case SguiZone.Codes.DoubleClick:
                    if (zone == zone_header)
                        fullscreen.Toggle();
                    break;

                case SguiZone.Codes.Exit:
                    if (drag_mode == 0)
                    {
                        CursorManager_OLD.UnsetUser(this);
                        cursor_mode = 0;
                    }
                    break;

                case SguiZone.Codes.Move:
                    if (drag_mode == 0)
                    {
                        if (zone == zone_header)
                            cursor_mode = CursorManager_OLD.Cursors.Move;

                        if (zone == zone_outline)
                            cursor_mode = DragmodeToCursor(data.position, false);

                        CursorManager_OLD.SetUser(this, cursor_mode);
                    }
                    break;

                case SguiZone.Codes.BeginDrag:
                    if (zone == zone_outline)
                        cursor_mode = DragmodeToCursor(data.position, true);
                    else if (zone == zone_header)
                    {
                        drag_mode = DRAG_MODES.ALL;
                        cursor_mode = CursorManager_OLD.Cursors.Move;
                        CursorManager_OLD.SetUser(this, cursor_mode);
                    }
                    break;

                case SguiZone.Codes.Drag:
                    if (zone == zone_header)
                        OnHeaderDrag(data.delta);
                    else if (zone == zone_outline)
                        OnSizeDrag(data.delta);
                    break;

                case SguiZone.Codes.EndDrag:
                    drag_mode = 0;
                    cursor_mode = 0;
                    CursorManager_OLD.UnsetUser(this);
                    break;
            }
        }

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
            if (fullscreen.Value)
                return;

            rT.anchoredPosition += ScreenDeltaToLocal(delta);
            CheckBounds();
        }

        public void CheckBounds()
        {
            (Vector2 pmin, Vector2 pmax) = rT_parent.GetWorldCorners();
            (Vector2 min, Vector2 max) = rT.GetWorldCorners();

            Vector2 move = Vector2.zero;

            if (min.x < pmin.x)
                move.x += pmin.x - min.x;
            if (max.x > pmax.x)
                move.x -= max.x - pmax.x;

            if (min.y < pmin.y)
                move.y += pmin.y - min.y;
            if (max.y > pmax.y)
                move.y -= max.y - pmax.y;

            if (move != default)
                rT.position += (Vector3)move;

            OnCheckBounds();
        }

        protected virtual void OnCheckBounds()
        {
        }

        CursorManager_OLD.Cursors DragmodeToCursor(Vector2 mouse_pos, in bool refresh_dragmode)
        {
            Vector2 size = rT.rect.size;

            mouse_pos = ScreenPositionToLocal(mouse_pos);

            mouse_pos.x /= size.x;
            mouse_pos.y /= size.y;

            DRAG_MODES drag_mode = 0;

            if (mouse_pos.x >= .9f)
                drag_mode |= DRAG_MODES.RIGHT;
            if (mouse_pos.x <= .1f)
                drag_mode |= DRAG_MODES.LEFT;
            if (mouse_pos.y >= .9f)
                drag_mode |= DRAG_MODES.TOP;
            if (mouse_pos.y <= .1f)
                drag_mode |= DRAG_MODES.BOTTOM;

            if (refresh_dragmode)
                this.drag_mode = drag_mode;

            return drag_mode switch
            {
                DRAG_MODES.TOP or DRAG_MODES.BOTTOM => CursorManager_OLD.Cursors.Sizable_n,
                DRAG_MODES.RIGHT or DRAG_MODES.LEFT => CursorManager_OLD.Cursors.Sizable_e,
                DRAG_MODES.TOP_RIGHT or DRAG_MODES.BOTTOM_LEFT => CursorManager_OLD.Cursors.Sizable_ne,
                DRAG_MODES.BOTTOM_RIGHT or DRAG_MODES.TOP_LEFT => CursorManager_OLD.Cursors.Sizable_se,
                _ => 0,
            };
        }

        void OnSizeDrag(Vector2 delta)
        {
            if (fullscreen.Value)
                return;

            delta = 2 * ScreenDeltaToLocal(delta);

            Vector2 parent_size = rT_parent.GetWorldSize();
            (Vector2 min, Vector2 max) = rT.GetWorldCorners();

            if (drag_mode.HasFlag(DRAG_MODES.TOP))
                rT.sizeDelta += delta * Vector2.up;

            if (drag_mode.HasFlag(DRAG_MODES.RIGHT))
                rT.sizeDelta += delta * Vector2.right;

            if (drag_mode.HasFlag(DRAG_MODES.BOTTOM))
                rT.sizeDelta += delta * Vector2.down;

            if (drag_mode.HasFlag(DRAG_MODES.LEFT))
                rT.sizeDelta += delta * Vector2.left;

            CheckBounds();
        }
    }
}