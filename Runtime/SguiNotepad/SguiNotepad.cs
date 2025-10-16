using System.IO;
using TMPro;
using UnityEngine;

namespace _SGUI_
{
    public partial class SguiNotepad : SguiWindow1
    {
        internal HeaderDropdown dropdown_files;
        public TMP_InputField main_input_field;
        [SerializeField] protected TextMeshProUGUI footer_tmp;
        [SerializeField] protected string file_path;

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void OnAfterSceneLoad()
        {
            SguiGlobal.instance.button_notepad.software_type = typeof(SguiNotepad);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static string TryOpenNotepad(in string file_path, in bool create_if_none, out SguiNotepad instance)
        {
            if (!File.Exists(file_path))
                if (create_if_none)
                {
                    DirectoryInfo parent = Directory.GetParent(file_path);
                    if (!parent.Exists)
                        Directory.CreateDirectory(parent.FullName);
                    File.WriteAllText(file_path, string.Empty);
                }
                else
                {
                    instance = null;
                    return $"can not find file '{file_path}'\n";
                }
            instance = Util.InstantiateOrCreate<SguiNotepad>(SguiGlobal.instance.rt_windows);
            instance.Init_file(file_path);
            return null;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            sgui_softwarebutton ??= SguiGlobal.instance.button_notepad;

            main_input_field = transform.Find("rT/body/file_body/scroll_view/viewport/content/input_field").GetComponent<TMP_InputField>();
            footer_tmp = transform.Find("rT/footer/text").GetComponent<TextMeshProUGUI>();

            dropdown_files = transform.Find("rT/buttons/layout/button_Files").GetComponent<HeaderDropdown>();
            dropdown_files.onItemClick += OnClick_FilesDropdown;

            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected void Init_file(in string file_path)
        {
            footer_tmp.text = file_path;
            this.file_path = file_path;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            main_input_field.text = string.Empty;
            base.Start();
        }
    }
}