using _SGUI_.Explorer;
using System.IO;
using System.Linq;

namespace _SGUI_
{
    partial class SguiExplorerView
    {
        internal void RebuildHierarchy()
        {
            string selected_fsi = null;

            if (this.selected_fsi._value != null)
                selected_fsi = this.selected_fsi._value.current_fsi.FullName;

            var all_toggled_folders = GetComponentsInChildren<Button_Folder>(true)
                .Where(x => x.toggle._value)
                .Select(x => x.current_dir.FullName)
                .ToArray();

            root_folder.toggle.Value = false;
            root_folder.toggle.Value = true;

            foreach (string f in all_toggled_folders)
            {
                GoHere(new DirectoryInfo(f));
                ((Button_Folder)this.selected_fsi._value).toggle.Value = true;
            }

            if (selected_fsi != null)
                GoHere(new DirectoryInfo(selected_fsi));
        }
    }
}