using _ARK_;
using _UTIL_;
using System;
using System.Collections.Generic;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;

namespace _SGUI_
{
    public sealed partial class SguiCompletor : MonoBehaviour
    {
        [AutoStaticsCleanup] public static SguiCompletor instance;

        RectTransform rT_intel;
        UnityEngine.UI.ScrollRect scrollview;
        CompletorItem compl_prefab;
        readonly List<CompletorItem> completions = new();
        [SerializeField] int current_index;
        public int compl_start, compl_end;
        [SerializeField] Vector2 offset;
        public object owner { get; private set; }
        Action<string> onConfirm;

        public readonly ValueNotifier<bool> toggle = new();

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;

            rT_intel = (RectTransform)transform.Find("rT");
            offset = rT_intel.localPosition;
            scrollview = rT_intel.GetComponentInChildren<UnityEngine.UI.ScrollRect>(true);

            compl_prefab = rT_intel.transform.Find("scroll-view/viewport/content-layout").Find("button").GetComponent<CompletorItem>();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnEnable()
        {
            UsageManager.AddUser(this, UsageGroups.Keyboard, UsageGroups.Typing, UsageGroups.TrueMouse);
            IMGUI_global.instance.inputs_users.AddElement(OnIMGUIInputs);
        }

        private void OnDisable()
        {
            UsageManager.RemoveUser(this);
            IMGUI_global.instance.inputs_users.RemoveElement(OnIMGUIInputs);
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
                    if (!RectTransformUtility.RectangleContainsScreenPoint(rT_intel, Input.mousePosition, ArkUI.instance.cameraUI))
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
            owner = null;
            onConfirm = null;
            toggle.Value = false;
        }

        public void PopulateCompletions(in int compl_start, in int compl_end, in Vector3 position, in IEnumerable<string> completions, object owner = null, Action<string> onConfirm = null)
        {
            this.compl_start = compl_start;
            this.compl_end = compl_end;

            ClearIntellisense();
            this.owner = owner;
            this.onConfirm = onConfirm;
            toggle.Value = true;

            rT_intel.position = position;
            rT_intel.anchoredPosition += offset;

            foreach (string completion in completions ?? Array.Empty<string>())
            {
                CompletorItem clone = Instantiate(compl_prefab, compl_prefab.transform.parent);
                this.completions.Add(clone);
                clone.gameObject.SetActive(true);
                clone.label.text = completion;
            }

            toggle.Value = this.completions.Count > 0;
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(scrollview.content);
            scrollview.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Max(scrollview.viewport.rect.height, UnityEngine.UI.LayoutUtility.GetPreferredHeight(scrollview.content)));
            scrollview.StopMovement();
            scrollview.verticalNormalizedPosition = 1;
            SelectItem(0);
        }

        void ClearIntellisense()
        {
            for (int i = 0; i < completions.Count; i++)
            {
                completions[i].gameObject.SetActive(false);
                Destroy(completions[i].gameObject);
            }
            completions.Clear();
        }

        internal void OnClickItem(in CompletorItem item)
        {
            SelectItem(item);
            onConfirm?.Invoke(GetSelectedValue());
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
            current_index = Mathf.Clamp(index, 0, Mathf.Max(0, completions.Count - 1));
            for (int i = 0; i < completions.Count; i++)
                completions[i].ToggleSelect(i == current_index);

            if (completions.Count == 0)
                return;
            var bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(scrollview.viewport, completions[current_index].transform);
            if (bounds.max.y > scrollview.viewport.rect.yMax)
                scrollview.content.anchoredPosition -= Vector2.up * (bounds.max.y - scrollview.viewport.rect.yMax);
            else if (bounds.min.y < scrollview.viewport.rect.yMin)
                scrollview.content.anchoredPosition += Vector2.up * (scrollview.viewport.rect.yMin - bounds.min.y);
            scrollview.StopMovement();
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
            IMGUI_global.instance.inputs_users.RemoveElement(OnIMGUIInputs);
            if (this == instance)
                instance = null;
        }
    }
}
