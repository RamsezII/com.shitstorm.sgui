using _ARK_;
using _SGUI_.context_click;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_.osview
{
    public class OSHeaderButton : MonoBehaviour
    {
        public RectTransform rt;
        [SerializeField] Button button;
        public Traductable trad;
        public Action<ContextList> onContextList;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            rt = (RectTransform)transform;
            button = GetComponent<Button>();
            trad = GetComponentInChildren<Traductable>(true);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            button.onClick.AddListener(() =>
            {
                Vector2 minPos = rt.rect.min;
                minPos.x += 10;
                Vector2 listPos = rt.TransformPoint(minPos);
                var list = SguiContextClick.instance.InstantiateListHere(listPos);
                onContextList(list);
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