using System;
using TMPro;
using UnityEngine;

namespace _SGUI_
{
    public partial class SguiOpen : SguiWindow2
    {
        Action<string> on_done;

        TMP_InputField header_input;

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void OnAfterSceneLoad()
        {
            OSView.instance.AddSoftwareButton<SguiOpen>(new()
            {
                french = "Explorateur de fichiers",
                english = "Files explorer",
            });
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnAwake()
        {
            prefab_hierarchy_folder = transform.Find("rT/left-window/scroll-view/viewport/content_layout/button-folder").GetComponent<Button_Folder>();
            prefab_hierarchy_file = transform.Find("rT/left-window/scroll-view/viewport/content_layout/button-file").GetComponent<Button_File>();

            header_input = transform.Find("rT/input_path/input_text").GetComponent<TMP_InputField>();

            base.OnAwake();
        }
    }
}