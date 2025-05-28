using System.Collections.Generic;
using UnityEngine;

namespace _SGUI_
{
    partial class SguiCodium
    {
        RectTransform rT_intel;
        CompletorItem compl_prefab;

        //--------------------------------------------------------------------------------------------------------------

        void AwakeIntellisense()
        {
            rT_intel = (RectTransform)transform.Find("rT/completor/rT");
            compl_prefab = rT_intel.transform.Find("scroll-view/viewport/content-layout").Find("button").GetComponent<CompletorItem>();
        }

        //--------------------------------------------------------------------------------------------------------------

        void StartIntellisense()
        {
            compl_prefab.gameObject.SetActive(false);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected void ResetIntellisense()
        {
            ClearIntellisense();
            rT_intel.gameObject.SetActive(false);
        }

        protected void UpdateIntellisense(in Vector3 position, in IEnumerable<string> completions)
        {
            ClearIntellisense();

            rT_intel.gameObject.SetActive(completions != null);
            rT_intel.position = position;

            foreach (string completion in completions)
            {
                CompletorItem clone = Instantiate(compl_prefab, compl_prefab.transform.parent);
                clone.gameObject.SetActive(true);
                clone.label.text = completion;
            }
        }

        void ClearIntellisense()
        {
            foreach (CompletorItem item in compl_prefab.transform.parent.GetComponentsInChildren<CompletorItem>())
                Destroy(item.gameObject);
        }
    }
}