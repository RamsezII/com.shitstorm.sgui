using _ARK_;
using System.IO;
using System.Linq;

namespace _SGUI_
{
    partial class OSView
    {
        void AwakeSguiSettings()
        {
            button_user_settings.onClick.AddListener(() =>
            {
                var window = SguiWindow.CreatePrompt();
                window.trad_title.SetTraductions(new() { french = "Réglages Machine", english = "Machine Settings", });
                window.SetDialogButtons(SguiCancelTypes.Off, SguiConfirmTypes.Ok);

                var files = NUCLEOR.DFHome.EnumerateFiles("*.json.txt", SearchOption.AllDirectories);
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
                                subwindow.SetDialogButtons(SguiCancelTypes.Back, SguiConfirmTypes.Save);
                                subwindow.EditJSon(type, file.FullName);
                            });
                        }
            });

            button_home_settings.onClick.AddListener(() =>
            {
                var window = SguiWindow.CreatePrompt();
                window.trad_title.SetTraductions(new() { french = "Réglages Home", english = "Home Settings", });
                window.SetDialogButtons(SguiCancelTypes.Off, SguiConfirmTypes.Ok);

                foreach (var target in IHomeTexts._users.GroupBy(target => target.GetType()).Select(target => target.First()))
                {
                    var button = window.AddButton<SguiCustom_Button>();
                    button.trad_label.SetText(target.GetType().FullName);

                    button.button.onClick.AddListener(() =>
                    {
                        window.Oblivionize();
                        var subwindow = SguiWindow.CreatePrompt();
                        subwindow.trad_title.SetText(target.GetType().FullName);
                        subwindow.SetDialogButtons(SguiCancelTypes.Back, SguiConfirmTypes.Ok);
                        subwindow.EditHomeText(target);
                    });
                }
            });
        }
    }
}