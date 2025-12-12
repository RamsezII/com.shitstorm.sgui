using System;
using System.Collections.Generic;
using UnityEngine;

namespace _SGUI_.Monitor.Processes
{
    public class Entry : SectionChild, SguiContextClick.IUser
    {
        [SerializeField] EntryColumn prefab_column;
        public readonly List<EntryColumn> columns = new();
        public int columnCount;
        public Action<SguiContextClick_List> onContextClick;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            prefab_column = GetComponentInChildren<EntryColumn>(includeInactive: true);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            prefab_column.gameObject.SetActive(false);
        }

        //--------------------------------------------------------------------------------------------------------------

        internal EntryColumn AddColumn()
        {
            EntryColumn column = Instantiate(prefab_column, prefab_column.transform.parent);
            column.column_index = columnCount++;
            column.gameObject.SetActive(true);
            columns.Add(column);
            return column;
        }

        void SguiContextClick.IUser.OnSguiContextClick(SguiContextClick_List context_list)
        {
            onContextClick?.Invoke(context_list);
        }
    }
}