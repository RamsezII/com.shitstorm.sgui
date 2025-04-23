namespace _SGUI_
{
    internal class Button_File : Button_Hierarchy
    {


        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();
            button.onClick.AddListener(() => editor.OnFileSelection(this));
        }
    }
}