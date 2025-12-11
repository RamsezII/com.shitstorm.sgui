using _ARK_;
using _SGUI_.Monitor.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_.Monitor
{
    public class Section : MonoBehaviour, ResourcesPage.IResourcesSection
    {
        public SguiMonitor monitor;
        public Button button;
        public Traductable trad;
        RectTransform arrow_rt;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            monitor = GetComponentInParent<SguiMonitor>();
            button = GetComponent<Button>();
            trad = GetComponentInChildren<Traductable>();
            arrow_rt = (RectTransform)transform.Find("arrow");
        }
    }
}