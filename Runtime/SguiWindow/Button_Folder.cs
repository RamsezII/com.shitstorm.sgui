using System.Collections.Generic;
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
        }
    }
}