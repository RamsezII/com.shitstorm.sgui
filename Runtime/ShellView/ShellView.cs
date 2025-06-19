using TMPro;
using UnityEngine;

namespace _SGUI_
{
    internal sealed partial class ShellView : MonoBehaviour
    {
        public ShellText std_out, std_in;
        public TextMeshProUGUI tmp_progress;

        //----------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            std_out = transform.Find("scrollview/viewport/content_layout/std_out").GetComponent<ShellText>();
            std_in = transform.Find("scrollview/viewport/content_layout/std_out").GetComponent<ShellText>();
            tmp_progress = transform.Find("progress/text").GetComponent<TextMeshProUGUI>();
        }

        //----------------------------------------------------------------------------------------------------------

        private void Start()
        {

        }

        //----------------------------------------------------------------------------------------------------------

        private void OnDestroy()
        {

        }
    }
}