using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    partial class SguiGlobal
    {
        public interface ISguiGlobalLeftClick
        {
            void OnSguiGlobalLeftClick();
        }

        bool OnImguiInputs(Event e)
        {
            if (e.type == EventType.MouseDown)
            {
                List<RaycastResult> rc_results = new();

                rc_results.Clear();

                PointerEventData data = new(EventSystem.current)
                {
                    position = Input.mousePosition
                };

                raycaster_2D.Raycast(data, rc_results);

                if (rc_results.Count > 0)
                    switch (e.keyCode)
                    {
                        case KeyCode.Mouse0:
                            for (int i = 0; i < rc_results.Count; ++i)
                            {
                                var clickable = rc_results[i].gameObject.GetComponentInParent<ISguiGlobalLeftClick>();
                                if (clickable != null)
                                {
                                    clickable.OnSguiGlobalLeftClick();
                                    return true;
                                }
                            }
                            break;
                    }
            }

            return false;
        }
    }
}