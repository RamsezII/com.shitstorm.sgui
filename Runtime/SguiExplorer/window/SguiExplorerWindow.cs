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

            view.selected_fsi.AddListener(fsi =>
            {
                if (view.selected_fsi.Has)
                    ChangeTitle(fsi.FullName);
                else
                    ChangeTitle(view.root_path);
            });
        }
    }
}