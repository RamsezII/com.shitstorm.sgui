using _ARK_;
using UnityEngine;

namespace _SGUI_
{
    public sealed class MouseUI : MonoBehaviour
    {
        public static MouseUI instance;

        RectTransform rt;

        //----------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;
            rt = (RectTransform)transform.Find("rT");
        }

        private void OnEnable()
        {
            NUCLEOR.delegates.LateUpdate += MoveCursor;
        }

        private void OnDisable()
        {
            NUCLEOR.delegates.LateUpdate -= MoveCursor;
        }

        //----------------------------------------------------------------------------------------------------------

        private void Start()
        {
            UsageManager.usages[(int)UsageGroups.GameMouse].AddListener1(this, gameObject.SetActive);
        }

        //----------------------------------------------------------------------------------------------------------

        void MoveCursor()
        {
            Vector2 local = rt.parent.InverseTransformPoint(NUCLEOR.instance.cursor_pos);
            rt.localPosition = local;
        }
    }
}