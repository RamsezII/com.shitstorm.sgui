using System;
using _UTIL_;
using UnityEngine;

namespace _SGUI_
{
    public partial class SguiOpen : SguiWindow2
    {
        [SerializeField] FS_TYPES mode;
        Action<string> on_done;

        //--------------------------------------------------------------------------------------------------------------

        public static SguiOpen InstantiatePrompt(in FS_TYPES mode, in Action<string> on_done)
        {
            SguiOpen clone = InstantiateWindow<SguiOpen>();
            clone.Init(on_done, mode);
            return clone;
        }

        void Init(in Action<string> on_done, in FS_TYPES mode)
        {
            this.mode = mode;
            this.on_done = on_done;
        }
    }
}