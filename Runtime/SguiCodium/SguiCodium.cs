using _ARK_;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public partial class SguiCodium : SguiNotepad
    {

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnAwake()
        {
            hierarchy_viewport_rT = (RectTransform)transform.Find("rT/body/left_explorer/hierarchy/scroll_view/viewport");
            hierarchy_content_rT = (RectTransform)hierarchy_viewport_rT.Find("content_layout");
            hierarchy_layout = hierarchy_content_rT.GetComponent<VerticalLayoutGroup>();

            prefab_hierarchy_folder = transform.Find("rT/body/left_explorer/hierarchy/scroll_view/viewport/content_layout/folder_button").GetComponent<Button_Folder>();
            prefab_hierarchy_file = transform.Find("rT/body/left_explorer/hierarchy/scroll_view/viewport/content_layout/file_button").GetComponent<Button_File>();

            base.OnAwake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            UsageManager.ToggleUser(this, true, UsageGroups.Typing, UsageGroups.IMGUI, UsageGroups.BlockPlayer, UsageGroups.Keyboard);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            UsageManager.RemoveUser(this);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            prefab_hierarchy_folder.gameObject.SetActive(false);
            prefab_hierarchy_file.gameObject.SetActive(false);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected void Init_folder(in string folder_path)
        {
            root_folder = NewFolder();
            root_folder.Init(folder_path, 0);
        }

        //--------------------------------------------------------------------------------------------------------------

        internal override void OnFile_click(in Button_File button_file)
        {
            script_view.file_path.Value = button_file.full_path.GetFile();
        }
    }
}