using _ARK_;
using _UTIL_;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    partial class SoftwareButton : SguiContextHover.IUser
    {

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
                        SguiContextHover.instance.SetTarget(this);
                    else
                        SguiContextHover.instance.UnsetTarget(this);
            };

            move_handler.onMove += (PointerEventData eventData) =>
            {
                if (software_instances.IsEmpty)
                    SguiContextHover.instance.SetTarget(this);
            };
        }

        //--------------------------------------------------------------------------------------------------------------

        Traductions SguiContextHover.IUser.OnSguiContextHover()
        {
            if (software_prefab == null)
                return new("???");
            return new(software_prefab.GetType().FullName);
        }
    }
}