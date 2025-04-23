using TMPro;
using UnityEngine;

namespace _SGUI_
{
    public partial class SguiNotepad : SguiWindow1
    {
        internal HeaderDropdown dropdown_files;
        public TMP_InputField main_input_field;
        [SerializeField] protected TextMeshProUGUI footer_tmp;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            main_input_field = transform.Find("rT/body/file_body/scroll_view/viewport/content/input_field").GetComponent<TMP_InputField>();
            footer_tmp = transform.Find("rT/footer/text").GetComponent<TextMeshProUGUI>();

            dropdown_files = transform.Find("rT/buttons/layout/button_Files").GetComponent<HeaderDropdown>();
            dropdown_files.onItemClick += OnClick_FilesDropdown;

            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            main_input_field.text = string.Empty;
            base.Start();
        }
    }
}