using _ARK_;
using _UTIL_;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    partial class SguiWindow
    {
        [SerializeField] internal RectTransform hierarchy_viewport_rT, hierarchy_content_rT;
        [SerializeField] internal VerticalLayoutGroup hierarchy_layout;

        [SerializeField] internal Button_Folder root_folder;
        [SerializeField] internal float hierarchy_width = 5;

        [SerializeField] internal FS_TYPES fs_mode = FS_TYPES.BOTH;
        [SerializeField] internal Button_Hierarchy hierarchy_last_select;

        internal Button_Folder NewFolder() => Instantiate(prefab_hierarchy_folder, prefab_hierarchy_folder.transform.parent);
        internal Button_File NewFile() => Instantiate(prefab_hierarchy_file, prefab_hierarchy_folder.transform.parent);

        //--------------------------------------------------------------------------------------------------------------

        internal virtual void OnFolder_click(in Button_Folder button_folder)
        {
            hierarchy_last_select = button_folder;
        }

        internal virtual void OnFile_click(in Button_File button_file)
        {
            hierarchy_last_select = button_file;
        }

        public void SetDirty_HierarchySize()
        {
            Util.AddAction(ref NUCLEOR.delegates.LateUpdate, ResizeHierarchy);
        }

        public void ResizeHierarchy()
        {
            if (oblivionized)
                return;

            Vector2 parent_size = hierarchy_viewport_rT.rect.size;
            Vector2 size = parent_size;
            Vector2 pref_size = new(hierarchy_layout.preferredWidth, hierarchy_layout.preferredHeight);

            if (pref_size.x > size.x)
                size.x = pref_size.x;
            if (pref_size.y > size.y)
                size.y = pref_size.y;

            hierarchy_content_rT.sizeDelta = size;
        }
    }
}