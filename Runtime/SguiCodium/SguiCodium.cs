using _ARK_;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public partial class SguiCodium : SguiNotepad
    {
        [SerializeField] protected TextMeshProUGUI lint_tmp;
        const int MAX_FILE_SIZE = 1024;
        string current_file_path;

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void OnAfterSceneLoad()
        {
            ArkMachine.AddListener(() =>
            {
                NUCLEOR.delegates.OnApplicationFocus += () => LoadSettings(false);
                NUCLEOR.delegates.OnApplicationUnfocus += () => SaveSettings(false);
            });
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            hierarchy_viewport_rT = (RectTransform)transform.Find("rT/body/left_explorer/hierarchy/scroll_view/viewport");
            hierarchy_content_rT = (RectTransform)hierarchy_viewport_rT.Find("content_layout");
            hierarchy_layout = hierarchy_content_rT.GetComponent<VerticalLayoutGroup>();

            prefab_hierarchy_folder = transform.Find("rT/body/left_explorer/hierarchy/scroll_view/viewport/content_layout/folder_button").GetComponent<Button_Folder>();
            prefab_hierarchy_file = transform.Find("rT/body/left_explorer/hierarchy/scroll_view/viewport/content_layout/file_button").GetComponent<Button_File>();

            base.Awake();

            lint_tmp = main_input_field.transform.Find("text_area/text/lint").GetComponent<TextMeshProUGUI>();
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
            main_input_field.onValueChanged.AddListener(OnValueChange);
            main_input_field.onValidateInput = OnValidateStdin;

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
            current_file_path = button_file.full_path;
            FileInfo file = new(current_file_path);
            if (file.Exists)
            {
                footer_tmp.text = file.FullName;
                if (file.Length <= MAX_FILE_SIZE)
                    main_input_field.text = File.ReadAllText(current_file_path);
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
            lint_tmp.text = text;
            Lint();
        }

        char OnValidateStdin(string text, int charIndex, char addedChar)
        {
            if (SguiCompletor.instance.toggle.Value)
                switch (addedChar)
                {
                    case ' ' when settings != null && settings.space_confirms_completion:
                    case '\n':
                    case '\t':
                        {
                            string completion = SguiCompletor.instance.GetSelectedValue();
                            if (!string.IsNullOrWhiteSpace(completion))
                            {
                                text = text[..SguiCompletor.instance.compl_start] + completion + text[SguiCompletor.instance.compl_end..];
                                main_input_field.text = text;
                                main_input_field.caretPosition = SguiCompletor.instance.compl_start + completion.Length;
                            }
                            SguiCompletor.instance.ResetIntellisense();
                        }
                        return '\0';
                }
            return addedChar;
        }

        void Lint() => Util.AddAction(ref NUCLEOR.delegates.LateUpdate_onEndOfFrame_once, OnLint);
        protected virtual void OnLint()
        {

        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDestroy()
        {
            base.OnDestroy();
            IMGUI_global.instance.users_inputs.RemoveKeysByValue(this);
        }
    }
}