using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    internal class Button_Folder : Button_Hierarchy
    {
        readonly List<Button_Hierarchy> hierarchy = new();

        public bool button_toggle;
        [SerializeField] RawImage icon_opened, icon_closed;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            icon_opened = transform.Find("offset/icon/opened").GetComponent<RawImage>();
            icon_closed = transform.Find("offset/icon/closed").GetComponent<RawImage>();
            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();
            button.onClick.AddListener(Toggle);
            Toggle(this == editor.root_folder);
        }

        //--------------------------------------------------------------------------------------------------------------

        public void Toggle() => Toggle(!button_toggle);
        public void Toggle(in bool toggle)
        {
            button_toggle = toggle;

            for (int i = 0; i < hierarchy.Count; ++i)
            {
                if (hierarchy[i] is Button_Folder bfolder)
                    bfolder.Toggle(false);
                Destroy(hierarchy[i].gameObject);
            }

            hierarchy.Clear();

            icon_opened.gameObject.SetActive(toggle);
            icon_closed.gameObject.SetActive(!toggle);

            editor.SetDirty_HierarchySize();

            if (toggle)
            {
                int depth = 1 + this.depth;
                foreach (string folder in Directory.EnumerateDirectories(full_path))
                {
                    Button_Folder clone = editor.NewFolder();
                    clone.Init(folder, depth);
                    hierarchy.Add(clone);
                }

                foreach (string file in Directory.EnumerateFiles(full_path))
                {
                    Button_File clone = editor.NewFile();
                    clone.Init(file, depth);
                    hierarchy.Add(clone);
                }

                int index = transform.GetSiblingIndex();
                for (int i = 0; i < hierarchy.Count; ++i)
                    hierarchy[i].transform.SetSiblingIndex(index + i + 1);
            }
        }
    }
}