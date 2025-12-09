using _ARK_;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public partial class SguiBootSelector : SguiWindow1
    {
        protected Traductable trad_sub_title;
        protected RectTransform content_rt;
        protected VerticalLayoutGroup vlayout;
        protected SguiBootEntry prefab_entry;
        protected SguiBootCategory[] categories;

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnAwake()
        {
            content_rt = (RectTransform)transform.Find("rT/body/content/inside/scrollview/viewport/content");
            vlayout = content_rt.Find("layout").GetComponent<VerticalLayoutGroup>();
            prefab_entry = GetComponentInChildren<SguiBootEntry>();
            categories = GetComponentsInChildren<SguiBootCategory>();
            trad_sub_title = transform.Find("rT/body/content/title").GetComponent<Traductable>();

            base.OnAwake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            prefab_entry.gameObject.SetActive(false);

            transform.SetSiblingIndex(SguiCompletor.instance.transform.GetSiblingIndex());
        }

        //--------------------------------------------------------------------------------------------------------------

        protected SguiBootEntry AddEntry()
        {
            SguiBootEntry entry = Instantiate(prefab_entry, prefab_entry.transform.parent);
            entry.gameObject.SetActive(true);
            return entry;
        }

        protected void ClearEntries()
        {
            var entries = transform.GetComponentsInChildren<SguiBootEntry>(true);
            foreach (var entry in entries)
                if (entry != prefab_entry)
                    Destroy(entry.gameObject);
        }
    }
}