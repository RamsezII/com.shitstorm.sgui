using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public class TaskbarButton_left : OSButton
    {
        [SerializeField] RawImage[] img_instances;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            img_instances = transform.Find("active").GetComponentsInChildren<RawImage>(true);
            base.Awake();
        }
    }
}