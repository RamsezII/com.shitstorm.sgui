using _ARK_;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _SGUI_
{
    partial class SguiWindow
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void InitShortcuts()
        {
            ArkShortcuts.AddShortcut_keyboard(
                shortcutName: "change focus",
                action: static () =>
                {

                },
                control: true,
                bindings: Key.Tab
            );
        }
    }
}