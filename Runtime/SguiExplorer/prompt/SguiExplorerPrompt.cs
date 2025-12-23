using System.IO;

namespace _SGUI_
{
    public partial class SguiExplorerPrompt : SguiWindow2
    {
        public SguiExplorerView view;

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnAwake()
        {
            view = GetComponentInChildren<SguiExplorerView>(true);

            base.OnAwake();
        }

        //--------------------------------------------------------------------------------------------------------------

        public static SguiExplorerPrompt Open()
        {
            var window = InstantiateWindow<SguiExplorerPrompt>();
            return window;
        }

        public static SguiExplorerPrompt OpenHere(in DirectoryInfo dir)
        {
            var window = InstantiateWindow<SguiExplorerPrompt>();
            window.view.GoHere(dir);
            return window;
        }
    }
}