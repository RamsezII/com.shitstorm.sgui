using _SGUI_.context_click;
using _UTIL_;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_.Explorer
{
    internal partial class Button_Folder : Button_Hierarchy
    {
        [SerializeField] RawImage icon_opened, icon_closed;

        public readonly ValueHandler<bool> toggle = new();

        public DirectoryInfo current_dir;
        internal readonly Dictionary<string, Button_Hierarchy> paths_buttons = new(StringComparer.Ordinal);

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            base.Awake();

            icon_opened = rt.Find("icon/opened").GetComponent<RawImage>();
            icon_closed = rt.Find("icon/closed").GetComponent<RawImage>();

            toggle.AddListener(value =>
            {
                icon_opened.gameObject.SetActive(value);
                icon_closed.gameObject.SetActive(!value);

                if (toggle._value)
                    view.selected_fsi.Value = this;

                Repopulate();
            });
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            transform.Find("rt/icon").GetComponent<PointerClickHandler>().onClick += eventData => toggle.Toggle();
        }

        //--------------------------------------------------------------------------------------------------------------

        public void Repopulate()
        {
            foreach (var path_button in paths_buttons)
            {
                if (path_button.Value is Button_Folder bfolder)
                    bfolder.toggle.Value = false;
                Destroy(path_button.Value.gameObject);
            }

            paths_buttons.Clear();

            if (toggle._value)
            {
                int sibling_index = transform.GetSiblingIndex();

                foreach (var fsi in current_dir.EnumerateFileSystemInfos("*", SearchOption.TopDirectoryOnly)
                    .OrderByDescending(x => x.Name, StringComparer.Ordinal)
                    .OrderBy(x => x is DirectoryInfo))
                {
                    Button_Hierarchy button = null;

                    if (fsi is DirectoryInfo dir)
                        button = view.prefab_folder.Clone(true);

                    if (fsi is FileInfo file)
                        button = view.prefab_file.Clone(true);

                    paths_buttons.Add(fsi.Name, button);

                    button.depth = 1 + depth;
                    button.AssignFsi(fsi);
                    button.transform.SetSiblingIndex(1 + sibling_index);
                }
            }

            view.AutoSize();
        }

        internal override void AssignFsi(in FileSystemInfo fsi)
        {
            base.AssignFsi(fsi);
            current_dir = (DirectoryInfo)fsi;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    if (eventData.clickCount == 2)
                        toggle.Toggle();
                    break;
            }
        }

        protected override void OnContextList(in ContextList list)
        {
            base.OnContextList(list);

            list.AddLine();

            {
                var button = list.AddButton(new()
                {
                    french = $"Créer un fichier",
                    english = $"Create file",
                });

                button.button.onClick.AddListener(() => view.Prompt_CreateFile(current_dir));
            }

            {
                var button = list.AddButton(new()
                {
                    french = $"Créer un dossier",
                    english = $"Create a directory",
                });

                button.button.onClick.AddListener(() => view.Prompt_CreateFolder(current_dir));
            }

            if (SguiExplorerView.onContextClick_directory != null)
            {
                list.AddLine();
                SguiExplorerView.onContextClick_directory(list, current_dir);
            }
        }
    }
}