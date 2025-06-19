namespace _SGUI_
{
    public sealed partial class SguiTerminal : SguiWindow1
    {
        internal ShellView shell_view;

        //----------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            shell_view = transform.Find("").GetComponent<ShellView>();
            base.Awake();
        }
    }
}