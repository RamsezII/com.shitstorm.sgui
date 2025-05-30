using _ARK_;
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

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;

            rT_intel = (RectTransform)transform;

            compl_prefab = rT_intel.transform.Find("rT/scroll-view/viewport/content-layout").Find("button").GetComponent<CompletorItem>();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnEnable()
        {
            UsageManager.AddUser(this, UsageGroups.Keyboard, UsageGroups.Typing, UsageGroups.IngameMouse, UsageGroups.TrueMouse);
            IMGUI_global.instance.users_inputs.AddElement(OnIMGUIInputs, this);
        }

        private void OnDisable()
        {
            UsageManager.RemoveUser(this);
            IMGUI_global.instance.users_inputs.RemoveElement(this);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            compl_prefab.gameObject.SetActive(false);
            SelectItem(-1);
        }

        //--------------------------------------------------------------------------------------------------------------

        bool OnIMGUIInputs(Event e)
        {
            if (e.isKey)
                switch (e.keyCode)
                {
                    case KeyCode.Escape:
                        Debug.Log("ESCAPE", this);
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
            rT_intel.gameObject.SetActive(false);
        }

        public void UpdateIntellisense(in Vector3 position, in IEnumerable<string> completions)
        {
            ClearIntellisense();

            rT_intel.gameObject.SetActive(completions != null);
            rT_intel.position = position;

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

        //--------------------------------------------------------------------------------------------------------------

        private void OnDestroy()
        {
            if (this == instance)
                instance = null;
        }
    }
}