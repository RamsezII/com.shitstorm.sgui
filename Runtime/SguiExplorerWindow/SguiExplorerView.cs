using _ARK_;
using _SGUI_.Explorer;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public partial class SguiExplorerView : ArkComponent, SguiContextClick.IUser
    {
        [SerializeField] VerticalLayoutGroup vlayout;
        [SerializeField] Button_Folder prefab_folder;
        [SerializeField] Button_File prefab_file;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            vlayout = GetComponentInChildren<VerticalLayoutGroup>(true);
            prefab_folder = GetComponentInChildren<Button_Folder>(true);
            prefab_file = GetComponentInChildren<Button_File>(true);

            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            prefab_folder.gameObject.SetActive(false);
            prefab_file.gameObject.SetActive(false);

            prefab_folder.Clone(true);
            prefab_folder.Clone(true);
            prefab_folder.Clone(true);
            prefab_folder.Clone(true);
            prefab_file.Clone(true);
            prefab_file.Clone(true);
            prefab_file.Clone(true);
            prefab_file.Clone(true);
            prefab_file.Clone(true);
        }

        //--------------------------------------------------------------------------------------------------------------

        public void AudotSize() => Util.AddAction(ref NUCLEOR.delegates.LateUpdate_onEndOfFrame_once, OnAutoSize);
        void OnAutoSize()
        {

        }

        public void OnSguiContextClick(SguiContextClick_List list)
        {
            {
                var button = list.AddButton();
                button.trad.SetTrads(new()
                {
                    french = $"Créer un fichier ici",
                    english = $"Create file here",
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