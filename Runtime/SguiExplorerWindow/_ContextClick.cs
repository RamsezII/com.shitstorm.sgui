namespace _SGUI_
{
    partial class SguiExplorerView : SguiContextClick.IUser
    {
        public void OnSguiContextClick(SguiContextClick_List list)
        {
            {
                var button = list.AddButton();
                button.trad.SetTrads(new()
                {
                    french = $"Créer un fichier",
                    english = $"Create file",
                });

                button.button.onClick.AddListener(Prompt_CreateFile);
            }

            {
                var button = list.AddButton();
                button.trad.SetTrads(new()
                {
                    french = $"Créer un dossier",
                    english = $"Create a directory",
                });

                button.button.onClick.AddListener(Prompt_CreateFolder);
            }

            {
                var button = list.AddButton();
                button.trad.SetTrads(new()
                {
                    french = $"Ouvrir un terminal ici",
                    english = $"Open a terminal here",
                });
            }

            {
                var button = list.AddButton();
                button.trad.SetTrads(new()
                {
                    french = $"Ouvrir Shitcodium ici",
                    english = $"Open Shitcodium here",
                });
            }
        }
    }
}