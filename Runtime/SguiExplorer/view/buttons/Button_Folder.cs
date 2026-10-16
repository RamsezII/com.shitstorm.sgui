using _SGUI_.context_click;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_.Explorer
{
    internal partial class Button_Folder : Button_Hierarchy
    {
        [SerializeField] RawImage icon_opened, icon_closed;
        internal DirectoryInfo current_dir;

        //--------------------------------------------------------------------------------------------------------------

        internal void Initialize(in DirectoryInfo directory)
        {
            base.Initialize(directory);
            current_dir = directory;
        }

        //--------------------------------------------------------------------------------------------------------------

        internal void Toggle(in bool value)
        {
            icon_opened.gameObject.SetActive(value);
            icon_closed.gameObject.SetActive(!value);
        }

        protected override void OnContextList(in ContextList list)
        {
            base.OnContextList(list);

            list.AddLine();

            {
                var button = list.AddButton_trad(new()
                {
                    french = $"Créer un fichier",
                    english = $"Create file",
                });

                button._button.onClick.AddListener(() => view.Prompt_CreateFile(current_dir));
            }

            {
                var button = list.AddButton_trad(new()
                {
                    french = $"Créer un dossier",
                    english = $"Create a directory",
                });

                button._button.onClick.AddListener(() => view.Prompt_CreateFolder(current_dir));
            }

            if (SguiExplorerView.onContextClick_directory != null)
            {
                list.AddLine();
                SguiExplorerView.onContextClick_directory(list, current_dir);
            }
        }
    }
}
