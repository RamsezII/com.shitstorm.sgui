using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace _SGUI_
{
    partial class OSView
    {
        public static readonly Dictionary<object, Action<SguiCustom>> onRuntimeSettingsPrompt = new();

        //--------------------------------------------------------------------------------------------------------------

        void AwakeRuntimeSettings()
        {
            rootGroup.transform.Find("task-bar/buttons-right/runtime/button").GetComponent<Button>().onClick.AddListener(() =>
            {
                var window = SguiWindow.CreatePrompt();
                window.SetDialogButtons(SguiCancelTypes.Off, SguiConfirmTypes.Ok);
                window.trad_title.SetText("Runtime");

                foreach (var pair in onRuntimeSettingsPrompt)
                {
                    var button = window.AddButton<SguiCustom_Button>();
                    button.trad_label.SetText(pair.Key.GetType().FullName);
                    button.button.onClick.AddListener(() =>
                    {
                        window.Oblivionize();
                        TryOpenRuntimeSettingsFromKey(pair.Key);
                    });
                }
            });
        }

        //--------------------------------------------------------------------------------------------------------------

        public static bool TryOpenRuntimeSettingsFromKey(in object key)
        {
            if (onRuntimeSettingsPrompt.TryGetValue(key, out var value))
            {
                var window = SguiWindow.CreatePrompt();

                window.SetDialogButtons(SguiCancelTypes.Back, SguiConfirmTypes.Apply);
                window.trad_title.SetText(key.GetType().FullName);

                value(window);

                if (!window.HasAnyButton)
                {
                    Destroy(window.gameObject);
                    return false;
                }

                return true;
            }
            return false;
        }
    }
}