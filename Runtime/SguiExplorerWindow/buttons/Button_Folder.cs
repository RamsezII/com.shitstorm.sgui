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
    internal class Button_Folder : Button_Hierarchy
    {
        [SerializeField] RawImage icon_opened, icon_closed;

        public readonly ValueHandler<bool> toggle = new();

        public DirectoryInfo current_dir;
        readonly List<Button_Hierarchy> children = new();

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            base.Awake();

            icon_opened = rt.Find("icon/opened").GetComponent<RawImage>();
            icon_closed = rt.Find("icon/closed").GetComponent<RawImage>();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            transform.Find("rt/icon").GetComponent<PointerClickHandler>().onClick += eventData => toggle.Toggle();

            toggle.AddListener(value =>
            {
                for (int i = 0; i < children.Count; ++i)
                {
                    if (children[i] is Button_Folder bfolder)
                        bfolder.toggle.Value = false;
                    Destroy(children[i].gameObject);
                }

                children.Clear();

                icon_opened.gameObject.SetActive(value);
                icon_closed.gameObject.SetActive(!value);

                if (value)
                {
                    view.selected_fsi.Value = this;

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

                        children.Add(button);

                        button.depth = 1 + depth;
                        button.AssignFsi(fsi);
                        button.transform.SetSiblingIndex(1 + sibling_index);
                    }
                }
            });
        }

        //--------------------------------------------------------------------------------------------------------------

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

        protected override void OnContextList(in SguiContextClick_List list)
        {
            base.OnContextList(list);

            {
                var button = list.AddButton();
                button.trad.SetTrads(new()
                {
                    french = $"Ouvrir dans Shitcodium",
                    english = $"Open in Shitcodium",
                });
            }
        }
    }
}