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
                instance.AssignUser(this);
            }

            void IPointerMoveHandler.OnPointerMove(PointerEventData eventData)
            {
                instance.AssignUser(this);
            }

            void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
            {
                instance.UnassignUser(this);
            }
        }

        public static SguiContextHover instance;

        Canvas canvas;
        Animator animator;
        RectTransform rt_all, rt_square;
        TextMeshProUGUI text;
        Traductable trad;
        [SerializeField] IUser user;
        Vector2 tpos;

        public bool Enabled => state_base == BaseStates.Enable;

        HeartBeat.Operation op;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;

            canvas = GetComponentInParent<Canvas>();
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

            trad.SetTrads(user.OnSguiContextHover());

            Vector2 psize = rt_all.rect.size;
            Vector2 size = text.GetPreferredValues(text.text, 200, float.MaxValue);
            size.x = Mathf.Max(size.x, 50);
            size.y = Mathf.Max(size.y, 15);

            rt_square.sizeDelta = size;
            rt_square.position = tpos;

            Vector2 pos = rt_square.anchoredPosition;
            pos.y += 5 + .5f * size.y;

            pos.x = Mathf.Clamp(pos.x, 5 + .5f * size.x, psize.x - .5f * size.x - 5);
            pos.y = Mathf.Clamp(pos.y, 5 + .5f * size.y, psize.y - .5f * size.y - 5);

            rt_square.anchoredPosition = pos;
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
                NUCLEOR.instance.heartbeat_unscaled.AddOperation(op = new(.15f, true, OnOperation));
        }

        public void AssignUser(in IUser user)
        {
            this.user = user;

            Toggle(false);

            tpos = Input.mousePosition;

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
                        animator.CrossFadeInFixedTime((int)BaseStates.Enable, .3f, (int)AnimLayers.Base);
                    break;

                case BaseStates.Enable:
                    if (!toggle)
                        animator.CrossFadeInFixedTime((int)BaseStates.Default, .1f, (int)AnimLayers.Base);
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