using _ARK_;
using _SGUI_.Monitor.Processes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public class ProcessesSorters : MonoBehaviour
    {
        public ProcessesSection section;
        public RectTransform rt;
        public HorizontalLayoutGroup hlayout;
        [SerializeField] SectionColumn prefab_column;
        internal readonly List<SectionColumn> columns = new();
        internal float init_height;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            section = GetComponentInParent<ProcessesSection>(includeInactive: true);
            rt = (RectTransform)transform;
            hlayout = GetComponentInChildren<HorizontalLayoutGroup>(includeInactive: true);
            prefab_column = GetComponentInChildren<SectionColumn>(includeInactive: true);
            init_height = rt.sizeDelta.y;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            prefab_column.gameObject.SetActive(false);
        }

        //--------------------------------------------------------------------------------------------------------------

        public SectionColumn AddColumn()
        {
            SectionColumn column = Instantiate(prefab_column, prefab_column.transform.parent);
            column.column_index = columns.Count;
            column.gameObject.SetActive(true);
            columns.Add(column);
            section.RefreshColumnWidth(column.column_index);
            AutoWidth();
            return column;
        }

        public void AutoWidth() => Util.AddAction(ref NUCLEOR.delegates.LateUpdate_onEndOfFrame_once, _AutoWidth);
        void _AutoWidth()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)hlayout.transform);
            rt.sizeDelta = new(hlayout.preferredWidth, init_height);
        }
    }
}