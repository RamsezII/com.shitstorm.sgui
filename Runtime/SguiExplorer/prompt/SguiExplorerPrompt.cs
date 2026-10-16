namespace _SGUI_
{
    public partial class SguiExplorerPrompt : SguiPrompt
    {
        public SguiExplorerView view;

        //--------------------------------------------------------------------------------------------------------------

        internal protected override void OnInitialize()
        {
            view = GetComponentInChildren<SguiExplorerView>(true);
            base.OnInitialize();
        }
    }
}