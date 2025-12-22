using _COBRA_;
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
                french = $"Shitplorateur",
                english = "Shitplorator",
            });
        }

        public static SguiExplorerWindow OpenHere(in PathModes mode)
        {
            return InstantiateWindow<SguiExplorerWindow>();
        }
    }
}