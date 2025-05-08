using UnityEngine;

namespace _SGUI_
{
    public static class CursorManager
    {
        public enum Cursors : byte
        {
            _none_,
            Clickable,
            Move,
            Sizable_n,
            Sizable_ne,
            Sizable_e,
            Sizable_se,
            _last_,
        }

        static readonly Sprite[] sprites = new Sprite[(int)Cursors._last_];

        static object current_user;
        static Cursors current_cursor;

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnBeforeSceneLoad()
        {
            current_user = null;
            current_cursor = 0;

            for (Cursors zone = 0; zone < Cursors._last_; ++zone)
            {
                string name = zone switch
                {
                    Cursors.Clickable => "sgui-cursor-clickable",
                    Cursors.Move => "sgui-cursor-move",
                    Cursors.Sizable_n => "sgui-cursor-resize-vertical",
                    Cursors.Sizable_ne => "sgui-cursor-resize-diagonal",
                    Cursors.Sizable_e => "sgui-cursor-resize-horizontal",
                    Cursors.Sizable_se => "sgui-cursor-resize-diagonal-mirrored",
                    _ => null,
                };
                if (name != null)
                    sprites[(int)zone] = Resources.Load<Sprite>(name);
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        public static void SetUser(in object user, in Cursors cursor)
        {
            current_user = user;
            current_cursor = cursor;
            AutoCursor();
        }

        public static void UnsetUser(in object user)
        {
            if (user == current_user)
            {
                current_user = null;
                current_cursor = 0;
            }
            AutoCursor();
        }

        static void AutoCursor()
        {
            Sprite sprite = sprites[(int)current_cursor];
            if (true || sprite == null)
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            else
                Cursor.SetCursor(sprite.texture, sprite.pivot, CursorMode.Auto);
        }
    }
}