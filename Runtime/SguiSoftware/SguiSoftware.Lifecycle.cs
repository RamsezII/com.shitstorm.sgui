using _ARK_;

namespace _SGUI_
{
    public abstract partial class SguiSoftware
    {
        protected override void OnToggleWindow(in bool toggle)
        {
            if (!toggle)
                ResizerVisual.instance?.UntakeFocus(this);

            base.OnToggleWindow(toggle);
        }

        protected override void OnDestroy()
        {
            if (IMGUI_global.instance != null)
                IMGUI_global.instance.inputs_users.RemoveElement(OnImguiInputs);

            ResizerVisual.instance?.UntakeFocus(this);

            fullscreen.Reset();
            fullscreen.Dispose();

            base.OnDestroy();
        }
    }
}
