using _ARK_;
using UnityEngine;

namespace _SGUI_
{
    public sealed partial class SguiGlobal : MonoBehaviour, ArkUI.IGuiGlobal
    {
        public static SguiGlobal instance;

        internal RectTransform rt_sgui_prompts;

#if UNITY_EDITOR
        [SerializeField] internal SguiWindow _FOCUSED_WINDOW;
        public RectTransform _FOCUSED_RECTT;
#endif

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;
            rt_sgui_prompts = (RectTransform)transform.Find("sgui_prompts");
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            if (IMGUI_global.instance != null)
                IMGUI_global.instance.inputs_users.AddElement(OnImguiInputs);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnDestroy()
        {
            if (IMGUI_global.instance != null)
                IMGUI_global.instance.inputs_users.RemoveElement(OnImguiInputs);

            if (instance == this)
                instance = null;
        }
    }
}
