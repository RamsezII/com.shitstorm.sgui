using System;
using System.IO;

namespace _SGUI_
{
    partial class SguiExplorerView
    {
        internal void Prompt_CreateFile(DirectoryInfo pdir)
        {
            var window = SguiWindow.InstantiateWindow<SguiCustom>();
            window.trad_title.SetTrads(new()
            {
                french = "Créer un fichier",
                english = "Create a file",
            });

            var inputfield = window.AddButton<SguiCustom_InputField>();
            inputfield.trad_label.SetTrads(new()
            {
                french = "Nom du fichier :",
                english = "File name:",
            });

            window.onFunc_confirm += () =>
            {
                string name = inputfield.input_field.text;
                if (string.IsNullOrEmpty(name))
                {
                    SguiWindow.ShowAlert(SguiDialogs.Error, out _, new()
                    {
                        french = "Donnez un nom au fichier",
                        english = "Choose a file name",
                    });
                    return false;
                }
                else
                {
                    try
                    {
                        selected_fsi.Value = null;
                        string final_path = Path.Combine(pdir.FullName, name);
                        File.WriteAllText(final_path, string.Empty);
                        RebuildHierarchy();
                        GoHere(new FileInfo(final_path));
                    }
                    catch (Exception ex)
                    {
                        SguiWindow.ShowAlert(SguiDialogs.Error, out _, new(ex.TrimmedExceptionMessage()));
                    }
                    return true;
                }
            };
        }

        internal void Prompt_CreateFolder(DirectoryInfo pdir)
        {
            var window = SguiWindow.InstantiateWindow<SguiCustom>();
            window.trad_title.SetTrads(new()
            {
                french = "Créer un dossier",
                english = "Create directory",
            });

            var inputfield = window.AddButton<SguiCustom_InputField>();
            inputfield.trad_label.SetTrads(new()
            {
                french = "Nom du dossier :",
                english = "Directory name:",
            });

            window.onFunc_confirm += () =>
            {
                string name = inputfield.input_field.text;
                if (string.IsNullOrEmpty(name))
                {
                    SguiWindow.ShowAlert(SguiDialogs.Error, out _, new()
                    {
                        french = "Choisissez un nom",
                        english = "Choose a name",
                    });
                    return false;
                }
                else
                {
                    try
                    {
                        selected_fsi.Value = null;
                        string final_path = Path.Combine(pdir.FullName, name);
                        Directory.CreateDirectory(final_path);
                        RebuildHierarchy();
                        GoHere(new DirectoryInfo(final_path));
                    }
                    catch (Exception ex)
                    {
                        SguiWindow.ShowAlert(SguiDialogs.Error, out _, new(ex.TrimmedExceptionMessage()));
                    }
                    return true;
                }
            };
        }
    }
}