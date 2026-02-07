using UnityEngine;

namespace _SGUI_.prompts.color_prompt
{
    internal sealed class ColorPromptGradient : MonoBehaviour
    {
        [SerializeField] SguiColorPrompt prompt;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            prompt = GetComponentInParent<SguiColorPrompt>(true);
        }
    }
}