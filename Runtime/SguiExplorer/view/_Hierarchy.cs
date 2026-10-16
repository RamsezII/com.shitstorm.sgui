using _ARK_;
using _SGUI_.Explorer;
using _UTIL_;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;

namespace _SGUI_
{
    partial class SguiExplorerView
    {
        [NoAutoStaticsCleanup] static readonly StringComparer pathComparer = Util.is_windows ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;

        [SerializeField] Button_Folder prefab_folder;
        [SerializeField] Button_File prefab_file;
        readonly Dictionary<string, Button_Hierarchy> clones = new(pathComparer);

        public string root_path;
        internal readonly HashSet<string> opened_folders = new(pathComparer);
        public readonly ValueNotifier<FileSystemInfo> selected_fsi = new();

        //--------------------------------------------------------------------------------------------------------------

        internal void RebuildHierarchy() => Util.AddActionOnce(ref NUCLEOR.delegates.LateUpdate_onEndOfFrame_once, RebuildHierarchy_now);
        void RebuildHierarchy_now()
        {
            if (this == null || !initialized)
                return;

            foreach (var clone in clones.Values)
            {
                clone.gameObject.SetActive(false);
                Destroy(clone.gameObject);
            }
            clones.Clear();

            PopulateDirectory(new DirectoryInfo(root_path), 0);

            // This rebuild already runs inside the end-of-frame callback.
            AutoSize_now();
        }

        void PopulateDirectory(DirectoryInfo directory, int depth)
        {
            string path = directory.FullName.NormalizePath();
            bool opened = opened_folders.Contains(path);

            var folder = prefab_folder.Clone(parent: vlayout.transform);
            folder.depth = depth;
            folder.Initialize(directory);
            folder.Toggle(opened);
            AddHierarchyRow(path, folder);

            if (!opened)
                return;

            FileSystemInfo[] entries;
            try
            {
                entries = directory.EnumerateFileSystemInfos().OrderBy(x => x is DirectoryInfo ? 0 : 1).ThenBy(x => x.Name, pathComparer).ToArray();
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                Debug.LogWarning($"Cannot read directory \"{directory.FullName}\": {ex.Message}", this);
                return;
            }

            foreach (var entry in entries)
            {
                if (entry is DirectoryInfo child)
                    PopulateDirectory(child, depth + 1);
                else if (entry is FileInfo file)
                {
                    var button = prefab_file.Clone(parent: vlayout.transform);
                    button.depth = depth + 1;
                    button.Initialize(file);
                    AddHierarchyRow(file.FullName.NormalizePath(), button);
                }
            }
        }

        void AddHierarchyRow(string path, Button_Hierarchy row)
        {
            clones.Add(path, row);
            row.transform.SetAsLastSibling();
            row.SetSelected(pathComparer.Equals(path, selected_fsi._value?.FullName.NormalizePath()));
            row.gameObject.SetActive(true);
        }

        internal void ToggleFolder(DirectoryInfo directory)
        {
            string path = directory.FullName.NormalizePath();
            if (!opened_folders.Add(path))
                opened_folders.Remove(path);

            RebuildHierarchy();
        }
    }
}
