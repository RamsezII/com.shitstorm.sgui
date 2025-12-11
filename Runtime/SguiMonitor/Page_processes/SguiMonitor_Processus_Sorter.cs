using _ARK_;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    public class SguiMonitor_Processus_Sorter : MonoBehaviour, SguiContextHover.IUser, IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler
    {
        public SguiMonitor_ProcessesPage page;
        public Traductable trad;
        public TextMeshProUGUI text;
        public Traductions hover_infos;
        Traductions SguiContextHover.IUser.OnSguiContextHover() => hover_infos;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            page = GetComponentInParent<SguiMonitor_ProcessesPage>();
            trad = GetComponentInChildren<Traductable>();
            text = GetComponentInChildren<TextMeshProUGUI>();
        }

        //--------------------------------------------------------------------------------------------------------------

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