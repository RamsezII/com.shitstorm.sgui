using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_.Monitor.Processes
{
    public class ProcessesSection : Section
    {
        public ScrollRect scrollview;
        public ProcessesSorters sorters;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            scrollview = GetComponentInChildren<ScrollRect>(includeInactive: true);
            sorters = GetComponentInChildren<ProcessesSorters>(includeInactive: true);
            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnToggle(in bool isOn)
        {
            base.OnToggle(isOn);
            scrollview.gameObject.SetActive(isOn);
        }

        protected override void OnElement(in SectionChild element)
        {
            base.OnElement(element);
            if (element is Entry entry)
                for (int i = 0; i < sorters.columns.Count; i++)
                    entry.AddColumn();
        }

        public void RefreshColumnWidth(in int index)
        {
            float width = sorters.columns[index].rt.sizeDelta.x;
            for (int i = 0; i < sorters.section.elements_clones.Count; ++i)
                if (sorters.section.elements_clones[i] is Entry entry)
                {
                    EntryColumn column = entry.columns[index];
                    column.rt.sizeDelta = new(width, column.init_height);
                }
            sorters.AutoWidth();
            AutoSize();
            page.AutoSize();
        }

        protected override void OnAutoSize()
        {
            if (this == null)
                return;

            if (!vlayout.gameObject.activeInHierarchy)
            {
                rt.sizeDelta = new(0, 20);
                return;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)sorters.hlayout.transform);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)vlayout.transform);

            float w = sorters.hlayout.preferredWidth;
            float h = vlayout.preferredHeight;

            scrollview.content.sizeDelta = new(w, h);

            rt.sizeDelta = new(0, 35 + h);
        }
    }
}