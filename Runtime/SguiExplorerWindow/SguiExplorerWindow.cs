using System.IO;
using UnityEngine;

namespace _SGUI_
{
    public partial class SguiExplorerWindow : SguiWindow2
    {
        public SguiExplorerView view;

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void OnAfterSceneLoad()
        {
            OSView.instance.AddSoftwareButton<SguiExplorerWindow>(new()
            {
                french = "Explorateur de fichiers",
                english = "Files explorer",
            });
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnAwake()
        {
            view = GetComponentInChildren<SguiExplorerView>(true);

            base.OnAwake();

            trad_title.SetTrads(new()
            {
                french = $"Explorateur",
                english = "Explorer",
            });
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();
        }

        //--------------------------------------------------------------------------------------------------------------

        public static SguiExplorerWindow OpenHere(in DirectoryInfo dir)
        {
            var window = InstantiateWindow<SguiExplorerWindow>();
            return window;
        }
    }
}