using _UTIL_;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_.Explorer
{
    internal class Button_Folder : Button_Hierarchy
    {
        readonly List<Button_Hierarchy> hierarchy = new();

        [SerializeField] RawImage icon_opened, icon_closed;

        public readonly ValueHandler<bool> toggle = new();

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

            toggle.AddListener(value =>
            {
                for (int i = 0; i < hierarchy.Count; ++i)
                {
                    if (hierarchy[i] is Button_Folder bfolder)
                        bfolder.toggle.Value = false;
                    Destroy(hierarchy[i].gameObject);
                }

                hierarchy.Clear();

                icon_opened.gameObject.SetActive(value);
                icon_closed.gameObject.SetActive(!value);
            });
        }

        //--------------------------------------------------------------------------------------------------------------

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
                    french = $"Ouvrir Shitcodium dans \"{short_path}\"",
                    english = $"Open Shitcodium in \"{short_path}\"",
                });
            }
        }
    }
}