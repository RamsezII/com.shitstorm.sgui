using _ARK_;
using _SGUI_.context_click;
using _SGUI_.Explorer;
using _UTIL_;
using System;
using System.IO;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_
{
    public partial class SguiExplorerView : ArkComponent1
    {
        [SerializeField] ScrollRect scrollview;
        [SerializeField] VerticalLayoutGroup vlayout;
        [AutoStaticsCleanup] public static Action<ContextList, FileSystemInfo> onContextList_fileAndDirectory;
        [AutoStaticsCleanup] public static Action<ContextList, FileInfo> onContextList_file;
        [AutoStaticsCleanup] public static Action<ContextList, DirectoryInfo> onContextList_directory;

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnEnable()
        {
            base.OnEnable();

            NUCLEOR.delegates.OnApplicationFocus += RebuildHierarchy;
            RebuildHierarchy();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            NUCLEOR.delegates.OnApplicationFocus -= RebuildHierarchy;
            NUCLEOR.delegates.LateUpdate_onEndOfFrame_once -= RebuildHierarchy_now;
            NUCLEOR.delegates.LateUpdate_onEndOfFrame_once -= AutoSize_now;
        }

        //--------------------------------------------------------------------------------------------------------------

        internal void Initialize()
        {
            scrollview = GetComponentInChildren<ScrollRect>(true);
            vlayout = GetComponentInChildren<VerticalLayoutGroup>(true);
            prefab_folder = GetComponentInChildren<Button_Folder>(true);
            prefab_file = GetComponentInChildren<Button_File>(true);

            prefab_folder.gameObject.SetActive(false);
            prefab_file.gameObject.SetActive(false);

            if (string.IsNullOrWhiteSpace(root_path))
                root_path = NUCLEOR.DFHome.FullName;

            root_path = new DirectoryInfo(root_path).FullName;
            opened_folders.Clear();
            opened_folders.Add(root_path);

            RebuildHierarchy();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            transform.Find("scroll-view/viewport/background").GetComponent<PointerClickHandler>().onClick += (PointerEventData eventData) =>
            {
                selected_fsi.Value = null;
                if (eventData.button == PointerEventData.InputButton.Right)
                    ((SguiContextList.IUser)this).OnSguiContextClick(eventData, SguiContextList.instance.InstantiateListAtScreenPoint(eventData.position, eventData.pressEventCamera));
            };

            selected_fsi.AddListener(RebuildHierarchy);
        }

        //--------------------------------------------------------------------------------------------------------------

        public void AutoSize() => Util.AddActionOnce(ref NUCLEOR.delegates.LateUpdate_onEndOfFrame_once, AutoSize_now);
        void AutoSize_now()
        {
            Vector2 psize = scrollview.viewport.rect.size;
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)vlayout.transform);
            Vector2 size = new(vlayout.preferredWidth, vlayout.preferredHeight);
            size = Vector2.Max(size, psize);
            scrollview.content.sizeDelta = size;
        }
    }
}
