using System.Collections.Generic;
using UnityEngine;

namespace _SGUI_
{
    public class SguiCompletor : MonoBehaviour
    {
        SguiCodium codium;
        RectTransform rT_intel;
        CompletorItem compl_prefab;
        readonly List<CompletorItem> completions = new();

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            codium = GetComponentInParent<SguiCodium>();
            rT_intel = (RectTransform)transform;

            compl_prefab = rT_intel.transform.Find("rT/scroll-view/viewport/content-layout").Find("button").GetComponent<CompletorItem>();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            compl_prefab.gameObject.SetActive(false);
            SelectItem(-1);
        }

        //--------------------------------------------------------------------------------------------------------------

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
        }

        void ClearIntellisense()
        {
            for (int i = 0; i < completions.Count; i++)
                Destroy(completions[i].gameObject);
            completions.Clear();
        }

        internal void OnClickItem(in CompletorItem item)
        {

        }

        internal void OnEnterItem(in CompletorItem item)
        {
            int index = completions.IndexOf(item);
            SelectItem(index);
        }

        void SelectItem(in int index)
        {
            for (int i = 0; i < completions.Count; i++)
                completions[i].ToggleSelect(i == index);
        }
    }
}