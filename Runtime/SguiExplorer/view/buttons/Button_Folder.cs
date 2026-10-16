using _SGUI_.context_click;
using _UTIL_;
using System;
using System.IO;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;
using UnityEngine.EventSystems;
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

            var icon = transform.Find("rt/icon");
            var clickHandler = icon.GetComponent<PointerClickHandler>();
            if (clickHandler == null)
                clickHandler = icon.gameObject.AddComponent<PointerClickHandler>();
            clickHandler.onClick += eventData =>
            {
                if (eventData.button == PointerEventData.InputButton.Left)
                    view.ToggleFolder(current_dir);
            };
        }

        //--------------------------------------------------------------------------------------------------------------

        internal void Toggle(in bool value)
        {
            icon_opened.gameObject.SetActive(value);
            icon_closed.gameObject.SetActive(!value);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (eventData.button == PointerEventData.InputButton.Left && eventData.clickCount == 2)
                view.ToggleFolder(current_dir);
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

            if (SguiExplorerView.onContextList_directory != null)
            {
                list.AddLine();
                SguiExplorerView.onContextList_directory(list, current_dir);
            }
        }
    }
}
