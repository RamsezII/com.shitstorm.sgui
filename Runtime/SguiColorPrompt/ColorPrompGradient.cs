using _UTIL_;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_.prompts.color_prompt
{
    internal sealed class ColorPromptGradient : MonoBehaviour
    {
        [SerializeField] SguiColorPrompt prompt;
        public TextMeshProUGUI label;
        public Slider _slider;
        public new UI_GradientRenderer renderer;
        public TMP_InputField inputField;
        [SerializeField] TextMeshProUGUI tmp_lint;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            prompt = GetComponentInParent<SguiColorPrompt>(true);
            label = transform.Find("label").GetComponent<TextMeshProUGUI>();
            _slider = transform.Find("slider").GetComponent<Slider>();
            renderer = GetComponentInChildren<UI_GradientRenderer>(true);
            inputField = transform.Find("inputfield").GetComponent<TMP_InputField>();
            tmp_lint = transform.Find("inputfield/text-area/lint").GetComponent<TextMeshProUGUI>();

            inputField.onValueChanged.AddListener(text =>
            {
                if (Util.TryParseFloat(text, out float value))
                    tmp_lint.text = Util.FloatToString(value);
                else
                    tmp_lint.text = text.SetColor(Color.red);
            });
        }
    }
}