namespace _SGUI_
{
    public abstract partial class SguiTerminal : SguiWindow1
    {
        protected override void OnAwake()
        {
            base.OnAwake();
            trad_title.SetTrad("SguiTerminal");
        }
    }
}