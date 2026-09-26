using _SGUI_.composer;
using UnityEngine;

namespace _SGUI_
{
    public sealed partial class SguiComposer : SguiSoftware
    {
        [SerializeField] SguiSplitView[] splitviews;

        //--------------------------------------------------------------------------------------------------------------

        internal SguiSplitView AddSplitView()
        {
            var view = Util.InstantiateOrCreate<SguiSplitView>(parent: rt_body);
            return view;
        }
    }
}