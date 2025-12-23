using System.IO;
using UnityEngine;

namespace _SGUI_.Explorer
{
    internal partial class Button_File : Button_Hierarchy
    {
        public FileInfo current_file;

        //--------------------------------------------------------------------------------------------------------------

        internal override void AssignFsi(in FileSystemInfo fsi)
        {
            base.AssignFsi(fsi);
            current_file = (FileInfo)fsi;
        }

        protected override void OnContextList(in SguiContextClick_List list)
        {
            base.OnContextList(list);

            DirectoryInfo pdir = Directory.GetParent(current_fsi.FullName);

            list.AddLine();

            {
                var button = list.AddButton();
                button.trad.SetTrads(new()
                {
                    french = $"Ouvrir fichier à l'exterieur",
                    english = $"Open file outside",
                });

                button.button.onClick.AddListener(() =>
                {
                    Application.OpenURL(current_file.FullName);
                });
            }

            list.AddLine();

            {
                var button = list.AddButton();
                button.trad.SetTrads(new()
                {
                    french = $"Créer un fichier",
                    english = $"Create file",
                });

                button.button.onClick.AddListener(() => view.Prompt_CreateFile(pdir));
            }

            {
                var button = list.AddButton();
                button.trad.SetTrads(new()
                {
                    french = $"Créer un dossier",
                    english = $"Create a directory",
                });

                button.button.onClick.AddListener(() => view.Prompt_CreateFolder(pdir));
            }

            if (SguiExplorerView.onContextClick_file != null)
            {
                list.AddLine();
                SguiExplorerView.onContextClick_file(list, current_file);
            }
        }
    }
}