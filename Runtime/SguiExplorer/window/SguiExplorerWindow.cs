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

            view.Initialize();

            view.selected_fsi.AddListener(fsi =>
            {
                if (view.selected_fsi.Has)
                    tab.trad_title.SetText(fsi.FullName);
                else
                    tab.trad_title.SetText(view.root_path);
            });
        }
    }
}