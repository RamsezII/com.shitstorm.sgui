using _SGUI_.context_click;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace _SGUI_.Monitor.Processes
{
    public class Entry : SectionChild, SguiContextList.IUser
    {
        [SerializeField] EntryColumn prefab_column;
        public readonly List<EntryColumn> columns = new();
        public int columnCount;
        public Action<ContextList> onContextClick;

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnInitialize()
        {
            prefab_column = GetComponentInChildren<EntryColumn>(includeInactive: true);
            base.OnInitialize();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            prefab_column.gameObject.SetActive(false);
            base.Start();
        }

        //--------------------------------------------------------------------------------------------------------------

        internal EntryColumn AddColumn()
        {
            EntryColumn column = prefab_column.Clone(false);
            column.Initialize();
            column.column_index = columnCount++;
            columns.Add(column);
            column.gameObject.SetActive(true);
            return column;
        }

        void SguiContextList.IUser.OnSguiContextClick(ContextList context_list)
        {
            onContextClick?.Invoke(context_list);
        }
    }
}