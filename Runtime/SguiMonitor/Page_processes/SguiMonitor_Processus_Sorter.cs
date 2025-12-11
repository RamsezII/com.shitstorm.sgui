using _ARK_;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    public class SguiMonitor_Processus_Sorter : MonoBehaviour, SguiContextHover.IUser, IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler
    {
        public SguiMonitor_ProcessesPage page;
        public RectTransform rt;
        public Traductable trad;
        public TextMeshProUGUI text;
        public Traductions hover_infos;
        Traductions SguiContextHover.IUser.OnSguiContextHover() => hover_infos;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            page = GetComponentInParent<SguiMonitor_ProcessesPage>();
            rt = (RectTransform)transform;
            trad = GetComponentInChildren<Traductable>();
            text = GetComponentInChildren<TextMeshProUGUI>();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnEnable()
        {
            Traductable.language.AddListener(OnLangage, doNotCallThisTime: !didStart);
        }

        private void OnDisable()
        {
            Traductable.language.RemoveListener(OnLangage);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            OnLangage(0);
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnLangage(Languages lang)
        {
            float w = text.GetPreferredValues(text.text, rt.sizeDelta.x, float.PositiveInfinity).x;
            w = Mathf.Min(w, rt.sizeDelta.x);
            text.rectTransform.sizeDelta = new(w, 0);
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            SguiContextHover.instance.SetTarget(this);
        }

        void IPointerMoveHandler.OnPointerMove(PointerEventData eventData)
        {
            SguiContextHover.instance.SetTarget(this);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            SguiContextHover.instance.UnsetTarget(this);
        }
    }
}