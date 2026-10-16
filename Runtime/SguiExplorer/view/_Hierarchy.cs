using _ARK_;
using _SGUI_.Explorer;
using _UTIL_;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace _SGUI_
{
    partial class SguiExplorerView
    {
        [SerializeField] Button_Folder prefab_folder;
        [SerializeField] Button_File prefab_file;

        public string root_path;
        internal readonly HashSet<DirectoryInfo> opened_folders = new();
        public readonly ValueNotifier<FileSystemInfo> selected_path = new();

        //--------------------------------------------------------------------------------------------------------------

        internal void RebuildHierarchy() => Util.AddActionOnce(ref NUCLEOR.delegates.LateUpdate_onEndOfFrame_once, RebuildHierarchy_now);
        void RebuildHierarchy_now()
        {

        }
    }
}