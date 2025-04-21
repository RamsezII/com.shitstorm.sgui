using _ARK_;
using System;
using System.IO;
using TMPro;
using UnityEngine;

namespace _SGUI_
{
    public partial class SguiEditor : SguiWindow
    {
        public string folder_path;

        public TMP_InputField main_input_field;
        [SerializeField] TextMeshProUGUI lint_tmp, footer_tmp;
        public Func<string, string> linter;

        [SerializeField] bool init_b;

        [SerializeField] Button_Folder prefab_folder;
        [SerializeField] Button_File prefab_file;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            base.Awake();

            main_input_field = transform.Find("rT/body/file_body/scroll_view/viewport/content/input_field").GetComponent<TMP_InputField>();
            lint_tmp = main_input_field.transform.Find("text_area/text/lint").GetComponent<TextMeshProUGUI>();
            footer_tmp = transform.Find("rT/footer/text").GetComponent<TextMeshProUGUI>();

            prefab_folder = transform.Find("rT/body/left_explorer/hierarchy/scroll_view/viewport/content_layout/folder_button").GetComponent<Button_Folder>();
            prefab_file = transform.Find("rT/body/left_explorer/hierarchy/scroll_view/viewport/content_layout/file_button").GetComponent<Button_File>();

            main_input_field.onValueChanged.AddListener(OnValueChange);
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
            base.Start();

            prefab_folder.gameObject.SetActive(false);
            prefab_file.gameObject.SetActive(false);

            IMGUI_global.instance.users_inputs.AddElement(OnImguiInput, this);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected void Init(in string folder_path)
        {
            this.folder_path = folder_path;
            footer_tmp.text = folder_path;
            init_b = true;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected virtual bool OnImguiInput(Event e)
        {
            if (e.type == EventType.KeyDown)
                if (e.alt || e.control || e.command)
                    if (e.keyCode == KeyCode.S)
                    {
                        Debug.Log("SAVE");
                        File.WriteAllText(folder_path, main_input_field.text);
                        return true;
                    }
            return false;
        }

        protected virtual void OnValueChange(string text)
        {
            if (linter != null)
                text = linter(text);
            lint_tmp.text = text;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDestroy()
        {
            base.OnDestroy();
            IMGUI_global.instance.users_inputs.RemoveElement(this);
        }
    }
}