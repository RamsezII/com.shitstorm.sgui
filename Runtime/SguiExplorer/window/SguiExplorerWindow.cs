using UnityEngine;

namespace _SGUI_
{
    public sealed class SguiExplorerWindow : SguiWindow1
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

        internal protected override void OnAwake()
        {
            view = GetComponentInChildren<SguiExplorerView>(true);

            base.OnAwake();

            trad_title.SetTrads(new()
            {
                french = $"Explorateur",
                english = "Explorer",
            });
        }
    }
}