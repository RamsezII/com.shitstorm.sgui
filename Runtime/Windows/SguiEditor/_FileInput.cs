using System.IO;

namespace _SGUI_
{
    partial class SguiEditor
    {
        internal void OnFileSelection(in Button_File button_file)
        {
            if (File.Exists(button_file.full_path))
                main_input_field.text = File.ReadAllText(button_file.full_path);
            else
                main_input_field.text = string.Empty;
        }
    }
}