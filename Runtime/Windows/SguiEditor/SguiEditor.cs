using _ARK_;
using System.IO;
using TMPro;
using UnityEngine;

namespace _SGUI_
{
    public partial class SguiEditor : SguiWindow
    {
        public string file_path;

        public TMP_InputField main_input_field;
        [SerializeField] TextMeshProUGUI lint_tmp, footer_tmp;

        [SerializeField] bool init_b;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            base.Awake();

            main_input_field = transform.Find("rT/body/file_body/Scroll View/Viewport/Content/InputField").GetComponent<TMP_InputField>();
            lint_tmp = transform.Find("rT/body/file_body/Scroll View/Viewport/Content/InputField/Text Area/Text/Lint").GetComponent<TextMeshProUGUI>();
            footer_tmp = transform.Find("rT/body/footer/text").GetComponent<TextMeshProUGUI>();
            main_input_field.onValueChanged.AddListener(OnValueChange);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();
            IMGUI_global.instance.users_inputs.AddElement(OnImguiInput, this);
        }

        //--------------------------------------------------------------------------------------------------------------

        public void Init(in string file_path, in bool create_if_none)
        {
            this.file_path = file_path;

            if (File.Exists(file_path))
                main_input_field.text = File.ReadAllText(file_path);
            else
                main_input_field.text = string.Empty;

            init_b = true;
            footer_tmp.text = file_path;
        }

        //--------------------------------------------------------------------------------------------------------------

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