using _SGUI_.Monitor;
using _SGUI_.Monitor.Processes;
using _SGUI_.Monitor.Resources;
using _UTIL_;
using UnityEngine;

namespace _SGUI_
{
    public class SguiMonitor : SguiSoftware
    {
        public enum Pages : byte
        {
            Processes,
            Resources,
            FileSystems,
        }

        public readonly ValueNotifier<Pages> page = new();

        public ProcessesPage page_processes;
        public ResourcesPage page_resources;

        [SerializeField] PageButton[] pages_buttons;

        //--------------------------------------------------------------------------------------------------------------

        internal static void AddSoftwareButton()
        {
            OSView.instance.AddSoftwareButton<SguiMonitor>(new()
            {
                french = "Moniteur de Ressources",
                english = "Resources Monitor",
            });
        }

        //--------------------------------------------------------------------------------------------------------------

        internal protected override void OnAwake()
        {
            page_processes = GetComponentInChildren<ProcessesPage>(includeInactive: true);
            page_resources = GetComponentInChildren<ResourcesPage>(includeInactive: true);
            pages_buttons = GetComponentsInChildren<PageButton>(includeInactive: true);

            base.OnAwake();

            trad_title.SetTraductions(new()
            {
                french = "Moniteur",
                english = "Monitor",
            });

            page_processes.OnAwake();
            page_resources.OnAwake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            for (int i = 0; i < pages_buttons.Length; i++)
            {
                var button = pages_buttons[i];
                Pages code = (Pages)i;
                button.button.onClick.AddListener(() => page.Value = code);
            }

            page.AddListener(value =>
            {
                page_processes.gameObject.SetActive(value == Pages.Processes);
                page_resources.gameObject.SetActive(value == Pages.Resources);

                for (int i = 0; i < pages_buttons.Length; ++i)
                    pages_buttons[i].img_selected.gameObject.SetActive((Pages)i == value);
            });
        }
    }
}