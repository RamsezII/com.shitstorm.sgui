using System.IO;

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

        //--------------------------------------------------------------------------------------------------------------

        public static SguiExplorerPrompt Open()
        {
            var window = ShowPrompt<SguiExplorerPrompt>();
            return window;
        }

        public static SguiExplorerPrompt OpenHere(in DirectoryInfo dir)
        {
            var window = ShowPrompt<SguiExplorerPrompt>();
            window.view.GoHere(dir);
            return window;
        }
    }
}