using _ARK_;
using _SGUI_.context_click;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_.window1
{
    public class HeaderButton : MonoBehaviour
    {
        public SguiWindow1 window;
        public RectTransform rt;
        public Button button;
        public Traductable trad;
        public Action<ContextList> onContextList;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            window = GetComponentInParent<SguiWindow1>(true);
            rt = (RectTransform)transform;
            button = GetComponent<Button>();
            trad = GetComponentInChildren<Traductable>(true);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            button.onClick.AddListener(() =>
            {
                if (onContextList == null)
                    return;

                Vector2 minPos = rt.rect.min;
                minPos += new Vector2(10, 0);
                Vector3 listPos = button.transform.TransformPoint(minPos);

                onContextList(SguiContextClick.instance.InstantiateListHere(listPos));
            });

            trad.onRefresh += AutoSize;
            _AutoSize_Immediate();
        }

        //--------------------------------------------------------------------------------------------------------------

        public void AutoSize() => Util.AddAction(ref NUCLEOR.delegates.LateUpdate_onEndOfFrame_once, _AutoSize_Immediate);
        void _AutoSize_Immediate()
        {
            rt.sizeDelta = new(trad.tmpro.preferredWidth, rt.sizeDelta.y);
        }
    }
}