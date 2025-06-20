namespace _SGUI_
{
    public partial class SguiTerminal : SguiWindow1
    {
        protected ShellView shell_view;

        //----------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            shell_view = GetComponentInChildren<ShellView>();
            base.Awake();
        }
    }
}