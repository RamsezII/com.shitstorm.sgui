using System.IO;
using _ARK_;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public partial class SguiEditor : SguiNotepad
    {
        [SerializeField] RectTransform content_parent_rT, content_rT;
        [SerializeField] VerticalLayoutGroup content_layout;

        [SerializeField] Button_Folder prefab_folder;
        [SerializeField] Button_File prefab_file;

        [SerializeField] internal Button_Folder root_folder;
        [SerializeField] internal float hierarchy_width = 5;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            base.Awake();

            content_parent_rT = (RectTransform)transform.Find("rT/body/left_explorer/hierarchy/scroll_view/viewport");
            content_rT = (RectTransform)content_parent_rT.Find("content_layout");
            content_layout = content_rT.GetComponent<VerticalLayoutGroup>();

            prefab_folder = transform.Find("rT/body/left_explorer/hierarchy/scroll_view/viewport/content_layout/folder_button").GetComponent<Button_Folder>();
            prefab_file = transform.Find("rT/body/left_explorer/hierarchy/scroll_view/viewport/content_layout/file_button").GetComponent<Button_File>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            USAGES.ToggleUser(this, true, UsageGroups.Typing, UsageGroups.TrueMouse, UsageGroups.IMGUI, UsageGroups.BlockPlayers, UsageGroups.Keyboard);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            USAGES.RemoveUser(this);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            main_input_field.onValueChanged.AddListener(OnValueChange);

            base.Start();

            prefab_folder.gameObject.SetActive(false);
            prefab_file.gameObject.SetActive(false);

            IMGUI_global.instance.users_inputs.AddElement(OnImguiInput, this);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected void Init(in string folder_path)
        {
            footer_tmp.text = folder_path;
            root_folder = NewFolder();
            root_folder.Init(folder_path, 0);
        }

        //--------------------------------------------------------------------------------------------------------------

        internal Button_Folder NewFolder() => Instantiate(prefab_folder, prefab_folder.transform.parent);
        internal Button_File NewFile() => Instantiate(prefab_file, prefab_folder.transform.parent);

        internal void OnFileSelection(in Button_File button_file)
        {
            if (File.Exists(button_file.full_path))
                main_input_field.text = File.ReadAllText(button_file.full_path);
            else
                main_input_field.text = string.Empty;
        }

        protected virtual bool OnImguiInput(Event e)
        {
            if (e.type == EventType.KeyDown)
                if (e.alt || e.control || e.command)
                    if (e.keyCode == KeyCode.S)
                    {
                        Debug.Log("SAVE");
                        return true;
                    }
            return false;
        }

        protected virtual void OnValueChange(string text)
        {

        }

        public void SetDirty_HierarchySize()
        {
            Util.AddAction(ref NUCLEOR.delegates.onLateUpdate, ResizeHierarchy);
        }

        public void ResizeHierarchy()
        {
            Vector2 parent_size = content_parent_rT.rect.size;
            Vector2 size = parent_size;
            Vector2 pref_size = new(content_layout.preferredWidth, content_layout.preferredHeight);

            if (pref_size.x > size.x)
                size.x = pref_size.x;
            if (pref_size.y > size.y)
                size.y = pref_size.y;

            content_rT.sizeDelta = size;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDestroy()
        {
            base.OnDestroy();
            IMGUI_global.instance.users_inputs.RemoveElement(this);
        }
    }
}