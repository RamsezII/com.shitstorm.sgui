#if HAS_BOA
using Boa;
using System;

namespace _SGUI_.context_click
{
    partial class ContextList : IContextList
    {
        void IContextList.AddButton(string label, Action action) => AddButton_string(label).action += action;
        void IContextList.AddLine() => AddLine();
        void IContextList.AddSublist(string label, Action<IContextList> onSublist) => AddButton_string(label).SetupSublist(onSublist);
    }
}
#endif