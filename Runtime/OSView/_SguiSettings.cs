using _ARK_;
using System;
using System.IO;

namespace _SGUI_
{
    partial class OSView
    {
        void AwakeSguiSettings()
        {
            button_bottom_settings.onClick.AddListener(() =>
            {
                var window = SguiWindow.CreatePrompt();
                window.trad_title.SetTraductions(new()
                {
                    french = "Réglages",
                    english = "Settings",
                });
                window.SetCancelButton(SguiCustom.CancelTypes.Off);
                window.SetConfirmButton(SguiCustom.ConfirmTypes.Ok);

                var files = ArkMachine.DFHome.EnumerateFiles("*.json.txt", SearchOption.AllDirectories);
                foreach (var file in files)
                    if (Util.TryCastType(file.Name[..^".json.txt".Length], out var type))
                        if (type.IsSubclassOf(typeof(JSon)))
                        {
                            var button = window.AddButton<SguiCustom_Button>();
                            button.trad_label.SetText(file.Name);
                            button.hover_infos = new(file.FullName);

                            button.button.onClick.AddListener(() =>
                            {
                                var subwindow = SguiWindow.CreatePrompt();
                                subwindow.trad_title.SetText(file.Name);
                                subwindow.SetCancelButton(SguiCustom.CancelTypes.Back);
                                subwindow.trad_confirm.SetTraductions(new() { french = "Sauvegarder", english = "Save", });

                                subwindow.EditJSon(file.FullName, type);
                            });
                        }
            });
        }
    }
}