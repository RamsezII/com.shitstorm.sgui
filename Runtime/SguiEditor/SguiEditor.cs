using System.IO;
using _ARK_;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public partial class SguiEditor : SguiNotepad
    {
        protected override void Awake()
        {
            base.Awake();

            hierarchy_viewport_rT = (RectTransform)transform.Find("rT/body/left_explorer/hierarchy/scroll_view/viewport");
            hierarchy_content_rT = (RectTransform)hierarchy_viewport_rT.Find("content_layout");
            hierarchy_layout = hierarchy_content_rT.GetComponent<VerticalLayoutGroup>();

            prefab_hierarchy_folder = transform.Find("rT/body/left_explorer/hierarchy/scroll_view/viewport/content_layout/folder_button").GetComponent<Button_Folder>();
            prefab_hierarchy_file = transform.Find("rT/body/left_explorer/hierarchy/scroll_view/viewport/content_layout/file_button").GetComponent<Button_File>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            USAGES.ToggleUser(this, true, UsageGroups.Typing, UsageGroups.TrueMouse, UsageGroups.IMGUI, UsageGroups.BlockPlayers, UsageGroups.Keyboard);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            USAGES.RemoveUser(this);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            main_input_field.onValueChanged.AddListener(OnValueChange);

            base.Start();

            prefab_hierarchy_folder.gameObject.SetActive(false);
            prefab_hierarchy_file.gameObject.SetActive(false);

            IMGUI_global.instance.users_inputs.AddElement(OnImguiInput, this);
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
            FileInfo file = new(button_file.full_path);
            if (file.Exists)
            {
                footer_tmp.text = file.FullName;
                if (file.Length <= 1024)
                    main_input_field.text = File.ReadAllText(button_file.full_path);
                else
                {
                    SguiAlert alert = SguiDialog.ShowDialog<SguiAlert>(out var routine);
                    alert.trad_title.SetTrad("[ALERT]");
                    alert.SetText(new($"{GetType().FullName} : file to big ({file.Length.LogDataSize()})\n{button_file.full_path.ToSubLog()}"));
                    NUCLEOR.instance.subScheduler.AddRoutine(routine);
                }
            }
            else
            {
                footer_tmp.text = string.Empty;
                main_input_field.text = string.Empty;
            }
        }

        protected virtual bool OnImguiInput(Event e)
        {
            if (e.type == EventType.KeyDown)
                if (e.alt || e.control || e.command)
                    if (e.keyCode == KeyCode.S)
                    {
                        Debug.Log("SAVE");
                        return true;
                    }
            return false;
        }

        protected virtual void OnValueChange(string text)
        {

        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDestroy()
        {
            base.OnDestroy();
            IMGUI_global.instance.users_inputs.RemoveElement(this);
        }
    }
}