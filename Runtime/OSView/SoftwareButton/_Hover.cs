using _ARK_;
using _UTIL_;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    partial class SoftwareButton : SguiContextHover.IUser
    {
        public Traductions hover_info;

        //--------------------------------------------------------------------------------------------------------------

        void StartHover()
        {
            RectTransform handlers_rt = (RectTransform)transform.Find("background");
            var enterExit_handler = handlers_rt.GetComponent<PointerEnterExitHandler>();
            var move_handler = handlers_rt.GetComponent<PointerMoveHandler>();

            bool mouseIsOnButton = false;

            enterExit_handler.onEnterExit += (PointerEventData eventData, bool onEnter) =>
            {
                mouseIsOnButton = onEnter;
                if (software_instances.IsEmpty)
                    if (onEnter)
                        SguiContextHover.instance.AssignUser(this);
                    else
                        SguiContextHover.instance.UnassignUser(this);
            };

            move_handler.onMove += (PointerEventData eventData) =>
            {
                if (software_instances.IsEmpty)
                    SguiContextHover.instance.AssignUser(this);
            };
        }

        //--------------------------------------------------------------------------------------------------------------

        Traductions SguiContextHover.IUser.OnSguiContextHover() => hover_info;
    }
}