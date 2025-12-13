using System;
using System.IO;
using _UTIL_;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        public static SguiOpen InstantiatePrompt(in FS_TYPES mode, in Action<string> on_done)
        {
            SguiOpen clone = InstantiateWindow<SguiOpen>();
            clone.Init(on_done, mode);
            return clone;
        }

        protected override void OnAwake()
        {
            hierarchy_viewport_rT = (RectTransform)transform.Find("rT/left-window/scroll-view/viewport");
            hierarchy_content_rT = (RectTransform)hierarchy_viewport_rT.Find("content_layout");
            hierarchy_layout = hierarchy_content_rT.GetComponent<VerticalLayoutGroup>();

            prefab_hierarchy_folder = transform.Find("rT/left-window/scroll-view/viewport/content_layout/button-folder").GetComponent<Button_Folder>();
            prefab_hierarchy_file = transform.Find("rT/left-window/scroll-view/viewport/content_layout/button-file").GetComponent<Button_File>();

            header_input = transform.Find("rT/input_path/input_text").GetComponent<TMP_InputField>();

            base.OnAwake();
        }

        void Init(in Action<string> on_done, in FS_TYPES mode)
        {
            fs_mode = mode;
            this.on_done = on_done;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            header_input.text = string.Empty;

            base.Start();

            foreach (DriveInfo drive_info in DriveInfo.GetDrives())
                if (drive_info.IsReady || true)
                {
                    Button_Folder folder_clone = NewFolder();
                    folder_clone.Init(drive_info.RootDirectory.FullName, 0);
                    folder_clone.text.text = folder_clone.short_path = drive_info.VolumeLabel;
                }

            onAction_confirm += OnSubmitButton;
        }

        //--------------------------------------------------------------------------------------------------------------

        internal override void OnFolder_click(in Button_Folder button_folder)
        {
            base.OnFolder_click(button_folder);

            if (button_folder == null)
                header_input.text = string.Empty;
            else
                header_input.text = button_folder.full_path;

            // populate content window
        }

        void OnSubmitButton()
        {
            if (hierarchy_last_select == null)
                on_done(null);
            else if (hierarchy_last_select is Button_Folder && fs_mode.HasFlag(FS_TYPES.DIRECTORY))
                on_done(hierarchy_last_select.full_path);
            else if (hierarchy_last_select is Button_File && fs_mode.HasFlag(FS_TYPES.FILE))
                on_done(hierarchy_last_select.full_path);
            else
                on_done(null);

            Oblivionize();
        }
    }
}