using System.IO;

namespace _SGUI_.Explorer
{
    internal class Button_File : Button_Hierarchy
    {
        public FileInfo current_file;

        //--------------------------------------------------------------------------------------------------------------

        internal override void AssignFsi(in FileSystemInfo fsi)
        {
            base.AssignFsi(fsi);
            current_file = (FileInfo)fsi;
        }

        protected override void OnContextList(in SguiContextClick_List list)
        {
            base.OnContextList(list);

            {
                var button = list.AddButton();
                button.trad.SetTrads(new()
                {
                    french = $"Éxecuter dans un terminal",
                    english = $"Execute in a terminal",
                });
            }

            {
                var button = list.AddButton();
                button.trad.SetTrads(new()
                {
                    french = $"Ouvrir avec",
                    english = $"Open with",
                });

                button.SetupSublist(sublist =>
                {
                    {
                        var button = sublist.AddButton();
                        button.trad.SetTrad("Shitpad");
                    }

                    {
                        var button = sublist.AddButton();
                        button.trad.SetTrad("Shitcodium");
                    }
                });
            }

            list.AddLine();

            view.OnSguiContextClick(list);
        }
    }
}