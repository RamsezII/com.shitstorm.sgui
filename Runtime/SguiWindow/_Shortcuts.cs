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
                action: static () =>
                {
                    if (openWindows._collection.Count > 0)
                        openWindows._collection[0].TakeFocus();
                },
                control: true,
                bindings: "tab"
            );
        }
    }
}