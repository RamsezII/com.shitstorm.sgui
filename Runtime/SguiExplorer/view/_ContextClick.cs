using _SGUI_.context_click;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    partial class SguiExplorerView : SguiContextList.IUser
    {
        public void OnSguiContextClick(PointerEventData eventData, ContextList list)
        {
            {
                var button = list.AddButton_trad(new()
                {
                    french = $"Créer un fichier",
                    english = $"Create file",
                });
            }

            {
                var button = list.AddButton_trad(new()
                {
                    french = $"Créer un dossier",
                    english = $"Create a directory",
                });
            }

            {
                var button = list.AddButton_trad(new()
                {
                    french = $"Ouvrir l'explorateur ici",
                    english = $"Open explorer here",
                });
            }
        }
    }
}