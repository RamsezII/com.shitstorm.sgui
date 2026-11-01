using Boa;
using System;

namespace _SGUI_.Boa
{
    public sealed partial class ContextList : IContextList
    {
        readonly context_click.ContextList list;

        //--------------------------------------------------------------------------------------------------------------

        public ContextList(context_click.ContextList list)
        {
            this.list = list;
        }

        //--------------------------------------------------------------------------------------------------------------

        void IContextList.AddButton(string label, Action action) => list.AddButton_string(label).action += action;
        void IContextList.AddLine() => list.AddLine();
        void IContextList.AddSublist(string label, Action<IContextList> onSublist) => list.AddButton_string(label).SetupSublist(sublist => onSublist(new ContextList(sublist)));
    }
}
