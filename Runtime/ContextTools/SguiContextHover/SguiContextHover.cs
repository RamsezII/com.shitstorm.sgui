using _ARK_;
using _UTIL_;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    public sealed partial class SguiContextHover : MonoBehaviour
    {
        public interface IUser : IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler
        {
            Traductions OnSguiContextHover();

            void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
            {
                if (eventData.dragging)
                    return;
                instance.AssignUser(this, eventData.position, eventData.enterEventCamera);
            }

            void IPointerMoveHandler.OnPointerMove(PointerEventData eventData)
            {
                if (this == instance.user)
                    instance.AssignUser(this, eventData.position, eventData.enterEventCamera);
            }

            void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
            {
                instance.UnassignUser(this);
            }
        }

        public static SguiContextHover instance;

        Animator animator;
        RectTransform rt_all, rt_square;
        TextMeshProUGUI text;
        Traductable trad;
        [SerializeField] IUser user;
        Vector2 tpos;
        Camera eventCamera;

        public bool Enabled => state_base == BaseStates.Enable;

        Scheduler.Operation op;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;

            animator = GetComponent<Animator>();

            rt_all = (RectTransform)transform;
            rt_square = (RectTransform)transform.Find("rt");

            text = rt_square.Find("text").GetComponent<TextMeshProUGUI>();
            trad = rt_square.Find("text").GetComponent<Traductable>();

            Toggle(true);
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnOperation()
        {
            op.Dispose();

            if (user == null)
            {
                user = null;
                Toggle(false);
                return;
            }

            Toggle(true);
            ToggleMouseCheck(true);

            trad.SetTraductions(user.OnSguiContextHover());

            Vector2 size = text.GetPreferredValues(text.text, 200, float.MaxValue);

            rt_square.sizeDelta = size;
            ArkUI.instance.SetScreenPosition(rt_square, tpos, eventCamera);
            rt_square.anchoredPosition += new Vector2(0, 5 + .5f * size.y);

            if (rt_square.GetStayInsideCorrection(rt_all, 5 * Vector2.one, out Vector2 correction))
                rt_square.position += (Vector3)correction;
        }

        void ToggleMouseCheck(in bool toggle)
        {
            NUCLEOR.delegates.Update_OnStartOfFrame -= CheckForMouseMove;
            if (toggle)
                NUCLEOR.delegates.Update_OnStartOfFrame += CheckForMouseMove;
        }

        void CheckForMouseMove()
        {
            if (user == null)
            {
                Toggle(false);
                ToggleMouseCheck(false);
            }

            if (Input.mousePositionDelta.sqrMagnitude > 0)
                UnassignUser(user);
        }

        void ToggleOperation(in bool toggle)
        {
            op?.Dispose();
            if (toggle)
                NUCLEOR.instance.scheduler_unscaled.AddOperation(op = new("wait before hover text", .25f, true, OnOperation));
        }

        public void AssignUser(in IUser user, in Vector2 screenPosition, Camera eventCamera)
        {
            if (string.IsNullOrWhiteSpace(user.OnSguiContextHover().GetAutomatic()))
                return;

            this.user = user;
            tpos = screenPosition;
            this.eventCamera = eventCamera;

            Toggle(false);

            ToggleOperation(true);
        }

        public void UnassignUser(in IUser user)
        {
            ToggleOperation(false);
            if (user == this.user)
            {
                this.user = null;
                Toggle(false);
            }
        }

        void Toggle(in bool toggle)
        {
            switch (state_base)
            {
                case BaseStates.Default:
                    if (toggle)
                        animator.CrossFadeInFixedTime((int)BaseStates.Enable, .4f, (int)AnimLayers.Base);
                    break;

                case BaseStates.Enable:
                    if (!toggle)
                        animator.CrossFadeInFixedTime((int)BaseStates.Default, .15f, (int)AnimLayers.Base);
                    break;
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnDestroy()
        {
            ToggleMouseCheck(false);
            op?.Dispose();
        }
    }
}
