using _ARK_;
using _SGUI_.Explorer;
using _UTIL_;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public partial class SguiExplorerView : ArkComponent, SguiContextClick.IUser
    {
        [SerializeField] VerticalLayoutGroup vlayout;
        [SerializeField] internal Button_Folder prefab_folder;
        [SerializeField] internal Button_File prefab_file;

        [SerializeField] internal Button_Folder home_folder;

        internal readonly ValueHandler<Button_Hierarchy> selected_fsi = new();

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            vlayout = GetComponentInChildren<VerticalLayoutGroup>(true);
            prefab_folder = GetComponentInChildren<Button_Folder>(true);
            prefab_file = GetComponentInChildren<Button_File>(true);

            base.Awake();

            home_folder = prefab_folder.Clone(true);
            home_folder.AssignFsi(ArkPaths.instance.Value.dpath_home.GetDir(true));
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            prefab_folder.gameObject.SetActive(false);
            prefab_file.gameObject.SetActive(false);
        }

        //--------------------------------------------------------------------------------------------------------------

        public void GoHere(in DirectoryInfo dir)
        {

        }

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
                    french = $"Ouvrir un terminal ici",
                    english = $"Open a terminal here",
                });
            }

            {
                var button = list.AddButton();
                button.trad.SetTrads(new()
                {
                    french = $"Ouvrir dans Shitcodium",
                    english = $"Open in Shitcodium",
                });
            }
        }
    }
}