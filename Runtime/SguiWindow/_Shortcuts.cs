using _ARK_;
using UnityEngine;

namespace _SGUI_
{
    partial class SguiWindow
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void InitShortcuts()
        {
            ArkShortcuts.AddShortcut(
                shortcutName: "change focus",
                nameof_button: "tab",
                action: static () =>
                {
                    if (focused._collection.Count > 0)
                        focused._collection[0].TakeFocus();
                },
                control: true
            );
        }
    }
}