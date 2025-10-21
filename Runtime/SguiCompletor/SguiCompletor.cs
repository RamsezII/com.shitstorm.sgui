using _ARK_;
using _UTIL_;
using System.Collections.Generic;
using UnityEngine;

namespace _SGUI_
{
    public class SguiCompletor : MonoBehaviour
    {
        public static SguiCompletor instance;

        RectTransform rT_intel;
        CompletorItem compl_prefab;
        readonly List<CompletorItem> completions = new();
        [SerializeField] int current_index;
        public int compl_start, compl_end;
        [SerializeField] Vector2 offset;

        public readonly OnValue_bool toggle = new();

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;

            rT_intel = (RectTransform)transform.Find("rT");
            offset = rT_intel.localPosition;

            compl_prefab = rT_intel.transform.Find("scroll-view/viewport/content-layout").Find("button").GetComponent<CompletorItem>();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnEnable()
        {
            UsageManager.AddUser(this, UsageGroups.Keyboard, UsageGroups.Typing, UsageGroups.GameMouse);
            IMGUI_global.instance.users_inputs.AddElement(OnIMGUIInputs, this);
        }

        private void OnDisable()
        {
            UsageManager.RemoveUser(this);
            IMGUI_global.instance.users_inputs.RemoveKeysByValue(this);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            compl_prefab.gameObject.SetActive(false);
            ResetIntellisense();
            toggle.AddListener(gameObject.SetActive);
        }

        //--------------------------------------------------------------------------------------------------------------

        bool OnIMGUIInputs(Event e)
        {
            switch (e.keyCode)
            {
                case KeyCode.Mouse0:
                case KeyCode.Mouse1:
                case KeyCode.Mouse2:
                case KeyCode.Mouse3:
                case KeyCode.Mouse4:
                case KeyCode.Mouse5:
                case KeyCode.Mouse6:
                    if (!RectTransformUtility.RectangleContainsScreenPoint(rT_intel, Input.mousePosition))
                        ResetIntellisense();
                    return true;

                case KeyCode.Escape:
                    ResetIntellisense();
                    return true;

                case KeyCode.UpArrow:
                case KeyCode.DownArrow:
                    if (e.keyCode == KeyCode.UpArrow)
                        SelectItem(current_index - 1);
                    if (e.keyCode == KeyCode.DownArrow)
                        SelectItem(current_index + 1);
                    return true;
            }
            return false;
        }

        public void ResetIntellisense()
        {
            ClearIntellisense();
            toggle.Value = false;
        }

        public void PopulateCompletions(in int compl_start, in int compl_end, in Vector3 position, in IEnumerable<string> completions)
        {
            this.compl_start = compl_start;
            this.compl_end = compl_end;

            ClearIntellisense();

            toggle.Value = completions != null;
            rT_intel.position = position + (Vector3)offset;

            foreach (string completion in completions)
            {
                CompletorItem clone = Instantiate(compl_prefab, compl_prefab.transform.parent);
                this.completions.Add(clone);
                clone.gameObject.SetActive(true);
                clone.label.text = completion;
            }

            SelectItem(0);
        }

        void ClearIntellisense()
        {
            for (int i = 0; i < completions.Count; i++)
                Destroy(completions[i].gameObject);
            completions.Clear();
        }

        internal void OnClickItem(in CompletorItem item)
        {
            SelectItem(item);
        }

        internal void OnSelectItem(in CompletorItem item)
        {
            SelectItem(item);
        }

        void SelectItem(in CompletorItem item)
        {
            int index = completions.IndexOf(item);
            SelectItem(index);
        }

        void SelectItem(in int index)
        {
            current_index = index;
            for (int i = 0; i < completions.Count; i++)
                completions[i].ToggleSelect(i == index);
        }

        public string GetSelectedValue()
        {
            if (current_index >= 0 && current_index < completions.Count)
                return completions[current_index].label.text;
            return string.Empty;
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnDestroy()
        {
            if (this == instance)
                instance = null;
        }
    }
}