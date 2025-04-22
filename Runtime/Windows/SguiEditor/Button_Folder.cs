using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

namespace _SGUI_
{
    internal class Button_Folder : Button_Hierarchy
    {
        public Toggle toggle;
        readonly List<Button_Hierarchy> hierarchy = new();

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            toggle = GetComponent<Toggle>();
            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnToggleValueChanged(bool isOn)
        {
            for (int i = 0; i < hierarchy.Count; ++i)
                Destroy(hierarchy[i].gameObject);
            hierarchy.Clear();

            if (isOn)
            {
                foreach (string folder in Directory.EnumerateDirectories(full_path))
                {
                    Button_Folder clone = editor.NewFolder();
                    clone.Init(folder);
                    hierarchy.Add(clone);
                }

                foreach (string file in Directory.EnumerateFiles(full_path))
                {
                    Button_File clone = editor.NewFile();
                    clone.Init(file);
                    hierarchy.Add(clone);
                }

                int index = transform.GetSiblingIndex();
                for (int i = 0; i < hierarchy.Count; ++i)
                    hierarchy[i].transform.SetSiblingIndex(index + i);
            }
        }
    }
}