using _SGUI_.context_click;
using _SGUI_.Explorer;
using System.IO;
using System.Linq;
using UnityEngine;

namespace _SGUI_
{
    partial class SguiExplorerView : SguiContextClick.IUser
    {
        public void OnSguiContextClick(ContextList list)
        {
            DirectoryInfo pdir = GetComponentsInChildren<Button_Folder>().Where(x => x.toggle._value).Last().current_dir;
            LoggerOverlay.Log($"view click: ({pdir.FullName})", this);

            {
                var button = list.AddButton();
                button.trad.SetTrads(new()
                {
                    french = $"Créer un fichier",
                    english = $"Create file",
                });

                button.button.onClick.AddListener(() => Prompt_CreateFile(pdir));
            }

            {
                var button = list.AddButton();
                button.trad.SetTrads(new()
                {
                    french = $"Créer un dossier",
                    english = $"Create a directory",
                });

                button.button.onClick.AddListener(() => Prompt_CreateFolder(pdir));
            }

            {
                var button = list.AddButton();
                button.trad.SetTrads(new()
                {
                    french = $"Ouvrir l'explorateur ici",
                    english = $"Open explorer here",
                });

                button.button.onClick.AddListener(() => Application.OpenURL(pdir.FullName));
            }
        }
    }
}