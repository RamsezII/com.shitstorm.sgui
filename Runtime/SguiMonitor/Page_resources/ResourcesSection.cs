using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_.Monitor.Resources
{
    public class ResourcesSection : Section
    {

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnToggle(in bool isOn)
        {
            base.OnToggle(isOn);
            vlayout.gameObject.SetActive(isOn);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnAutoSize()
        {
            if (this == null)
                return;

            if (!vlayout.gameObject.activeInHierarchy)
            {
                rt.sizeDelta = new(0, 20);
                return;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)vlayout.transform);

            float h = vlayout.preferredHeight;

            rt.sizeDelta = new(0, 30 + h);
        }
    }
}