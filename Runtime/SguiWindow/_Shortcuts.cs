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
                    switch (focused._collection.Count)
                    {
                        case 0:
                            break;

                        case 1:
                            focused._collection[0].TakeFocus();
                            break;

                        default:
                            focused._collection[0].TakeFocus();
                            break;
                    }
                },
                control: true
            );
        }
    }
}