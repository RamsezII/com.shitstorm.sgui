using _ARK_;
using _SGUI_.tab_control;
using _UTIL_;
using UnityEngine;

namespace _SGUI_
{
    public sealed class SguiTabController : ArkComponent
    {
        [SerializeField] SguiTabButton prefab_tab;
        readonly ListListener<SguiTabButton> tabs = new();

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            prefab_tab = GetComponentInChildren<SguiTabButton>(true);
            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            prefab_tab.gameObject.SetActive(false);

            tabs.AddListener2(list =>
            {
                for (int i = 0; i < list.Count; i++)
                    list[i].selected.Value = i < list.Count - 1;
            });
        }

        //--------------------------------------------------------------------------------------------------------------

        public SguiTabButton AddTab()
        {
            var tab = prefab_tab.Clone(true);
            tabs.AddElement(tab);

            tab.button_select.onClick.AddListener(() => tabs.AddElement(tab));
            tab.button_close.onClick.AddListener(() =>
            {
                Destroy(tab.gameObject);
                tabs.RemoveElement(tab);
            });

            return tab;
        }
    }
}