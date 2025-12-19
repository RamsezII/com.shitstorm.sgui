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

        protected override void Awake()
        {
            prefab_column = GetComponentInChildren<EntryColumn>(includeInactive: true);
            base.Awake();
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
            EntryColumn column = prefab_column.Clone(true);
            column.column_index = columnCount++;
            columns.Add(column);
            return column;
        }

        void SguiContextClick.IUser.OnSguiContextClick(SguiContextClick_List context_list)
        {
            onContextClick?.Invoke(context_list);
        }
    }
}