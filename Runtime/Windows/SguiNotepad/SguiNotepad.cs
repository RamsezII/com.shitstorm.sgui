using TMPro;
using UnityEngine;

namespace _SGUI_
{
    public partial class SguiNotepad : SguiWindow
    {
        public TMP_InputField main_input_field;
        [SerializeField] protected TextMeshProUGUI footer_tmp;

        //--------------------------------------------------------------------------------------------------------------
        
        protected override void Awake()
        {
            main_input_field = transform.Find("rT/body/file_body/scroll_view/viewport/content/input_field").GetComponent<TMP_InputField>();
            footer_tmp = transform.Find("rT/footer/text").GetComponent<TextMeshProUGUI>();

            base.Awake();
        }
    }
}