using _SGUI_.composer;

namespace _SGUI_
{
    public sealed class SguiExplorerWindow : SguiFrame
    {
        public SguiExplorerView view;

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnInitialize()
        {
            view = GetComponentInChildren<SguiExplorerView>(true);

            base.OnInitialize();
        }
    }
}