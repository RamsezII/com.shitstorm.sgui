using _ARK_;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public partial class SguiEditor : SguiNotepad
    {
        [SerializeField] protected TextMeshProUGUI lint_tmp;
        float lint_last;
        bool lint_flag;
        const float lint_timer = .2f;
        const int MAX_FILE_SIZE = 1024;
        string current_file_path;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            base.Awake();

            hierarchy_viewport_rT = (RectTransform)transform.Find("rT/body/left_explorer/hierarchy/scroll_view/viewport");
            hierarchy_content_rT = (RectTransform)hierarchy_viewport_rT.Find("content_layout");
            hierarchy_layout = hierarchy_content_rT.GetComponent<VerticalLayoutGroup>();

            prefab_hierarchy_folder = transform.Find("rT/body/left_explorer/hierarchy/scroll_view/viewport/content_layout/folder_button").GetComponent<Button_Folder>();
            prefab_hierarchy_file = transform.Find("rT/body/left_explorer/hierarchy/scroll_view/viewport/content_layout/file_button").GetComponent<Button_File>();

            lint_tmp = main_input_field.transform.Find("text_area/text/lint").GetComponent<TextMeshProUGUI>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            UsageManager.ToggleUser(this, true, UsageGroups.Typing, UsageGroups.TrueMouse, UsageGroups.IMGUI, UsageGroups.BlockPlayers, UsageGroups.Keyboard);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            UsageManager.RemoveUser(this);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            main_input_field.onValueChanged.AddListener(OnValueChange);

            base.Start();

            prefab_hierarchy_folder.gameObject.SetActive(false);
            prefab_hierarchy_file.gameObject.SetActive(false);

            IMGUI_global.instance.users_inputs.AddElement(OnImguiInput, this);

            NUCLEOR.delegates.shell_tick += UpdateLintTimer;
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
            current_file_path = button_file.full_path;
            FileInfo file = new(current_file_path);
            if (file.Exists)
            {
                footer_tmp.text = file.FullName;
                if (file.Length <= MAX_FILE_SIZE)
                {
                    main_input_field.text = File.ReadAllText(current_file_path);
                    lint_flag = true;
                }
                else
                {
                    SguiCustom sgui = InstantiateWindow<SguiCustom>();
                    var alert = sgui.AddButton<SguiCustom_Alert>();
                    alert.SetText(new($"{GetType().FullName} : file to big ({file.Length.LogDataSize()})\n{current_file_path.ToSubLog()}"));
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
                        Debug.Log($"Saved file: \"{current_file_path}\"", this);
                        File.WriteAllText(current_file_path, main_input_field.text);
                        return true;
                    }
            return false;
        }

        protected virtual void OnValueChange(string text)
        {
            lint_flag = true;
            ResetLintTimer();
        }

        void ResetLintTimer()
        {
            lint_last = Time.unscaledTime;
        }

        void UpdateLintTimer()
        {
            if (lint_flag)
                if (Time.unscaledTime - lint_timer > lint_last)
                {
                    ResetLintTimer();
                    lint_flag = false;
                    OnLint();
                }
        }

        protected virtual void OnLint()
        {

        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDestroy()
        {
            base.OnDestroy();
            IMGUI_global.instance.users_inputs.RemoveElement(this);
            NUCLEOR.delegates.shell_tick -= UpdateLintTimer;
        }
    }
}