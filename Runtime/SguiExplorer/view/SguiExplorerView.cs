using _ARK_;
using _SGUI_.context_click;
using _SGUI_.Explorer;
using _UTIL_;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_
{
    public partial class SguiExplorerView : ArkComponent
    {
        [SerializeField] ScrollRect scrollview;
        [SerializeField] VerticalLayoutGroup vlayout;
        [SerializeField] internal Button_Folder prefab_folder;
        [SerializeField] internal Button_File prefab_file;

        public string root_dpath;
        [SerializeField] internal Button_Folder root_folder;

        internal readonly ValueHandler<Button_Hierarchy> selected_fsi = new();

        public static Action<ContextList, DirectoryInfo> onContextClick_directory;
        public static Action<ContextList, FileInfo> onContextClick_file;

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatics()
        {
            onContextClick_directory = null;
            onContextClick_file = null;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            scrollview = GetComponentInChildren<ScrollRect>(true);
            vlayout = GetComponentInChildren<VerticalLayoutGroup>(true);
            prefab_folder = GetComponentInChildren<Button_Folder>(true);
            prefab_file = GetComponentInChildren<Button_File>(true);

            base.Awake();

            root_dpath = ArkPaths.instance.Value.dpath_home;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            InitRootFolder();

            prefab_folder.gameObject.SetActive(false);
            prefab_file.gameObject.SetActive(false);

            transform.Find("scroll-view/viewport/background").GetComponent<PointerClickHandler>().onClick += (PointerEventData eventData) =>
            {
                selected_fsi.Value = null;
                if (eventData.button == PointerEventData.InputButton.Right)
                    ((SguiContextClick.IUser)this).OnSguiContextClick(SguiContextClick.instance.InstantiateListHere(eventData.position));
            };

            NUCLEOR.delegates.OnApplicationFocus += RebuildHierarchy;
        }

        //--------------------------------------------------------------------------------------------------------------

        void InitRootFolder()
        {
            if (root_folder != null)
                return;

            root_folder = prefab_folder.Clone(true);
            root_folder.AssignFsi(root_dpath.GetDir(false));
            root_folder.toggle.Value = true;
        }

        public bool TryGetSelectedFSI(out FileSystemInfo fsi)
        {
            if (selected_fsi._value != null)
            {
                fsi = selected_fsi._value.current_fsi;
                return true;
            }
            fsi = null;
            return false;
        }

        public bool TryGetSelectedDIR(out DirectoryInfo dir)
        {
            if (selected_fsi._value is Button_Folder bdir)
            {
                dir = bdir.current_dir;
                return true;
            }
            dir = null;
            return false;
        }

        public bool TryGetSelectedFILE(out FileInfo file)
        {
            if (selected_fsi._value is Button_File bfile)
            {
                file = bfile.current_file;
                return true;
            }
            file = null;
            return false;
        }

        public void GoHere(in FileSystemInfo fsi)
        {
            if (root_folder == null)
                InitRootFolder();

            string target_path = Path.GetFullPath(fsi.FullName);
            string root_path = Path.GetFullPath(root_folder.current_dir.FullName);

            if (target_path.StartsWith(root_path, StringComparison.Ordinal))
            {
                string rel_path = Path.GetRelativePath(root_path, target_path);
                if (rel_path == ".")
                {
                    root_folder.toggle.Value = true;
                    return;
                }

                var current = root_folder;

                var splits = rel_path.Split(
                    new char[]
                    {
                        Path.DirectorySeparatorChar,
                        Path.AltDirectorySeparatorChar,
                    },
                    StringSplitOptions.RemoveEmptyEntries
                );

                for (int i = 0; i < splits.Length; i++)
                {
                    string split = splits[i];
                    current.toggle.Value = true;

                    if (!current.paths_buttons.TryGetValue(split, out var button))
                        Debug.LogWarning($"path \"{split}\" is not present ({selected_fsi._value.current_fsi.FullName})", this);
                    else if (i == splits.Length - 1)
                    {
                        if (button is Button_Folder bdir)
                            bdir.toggle.Value = true;
                        selected_fsi.Value = button;
                    }
                    else
                        current = (Button_Folder)current.paths_buttons[split];
                }
            }
        }

        public void AutoSize() => Util.AddAction(ref NUCLEOR.delegates.LateUpdate_onEndOfFrame_once, OnAutoSize);
        void OnAutoSize()
        {
            Vector2 psize = scrollview.viewport.rect.size;
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)vlayout.transform);
            Vector2 size = new(vlayout.preferredWidth, vlayout.preferredHeight);
            size = Vector2.Max(size, psize);
            scrollview.content.sizeDelta = size;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDestroy()
        {
            NUCLEOR.delegates.OnApplicationFocus -= RebuildHierarchy;
            base.OnDestroy();
        }
    }
}