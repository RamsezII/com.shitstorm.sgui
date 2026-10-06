using UnityEngine;
using _SGUI_.composer;

namespace _SGUI_
{
    public sealed class SguiExplorerWindow : SguiFrame
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

        protected override void OnInitialize()
        {
            view = GetComponentInChildren<SguiExplorerView>(true);

            base.OnInitialize();
        }
    }
}