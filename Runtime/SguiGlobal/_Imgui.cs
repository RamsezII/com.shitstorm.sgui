using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    partial class SguiGlobal
    {
        bool OnImguiInputs(Event e)
        {
            if (e.type == EventType.MouseDown)
                switch (e.keyCode)
                {
                    case KeyCode.Mouse0:
                        {
                            List<RaycastResult> rc_results = new();

                            rc_results.Clear();

                            PointerEventData data = new(EventSystem.current)
                            {
                                position = Input.mousePosition
                            };

                            raycaster_2D.Raycast(data, rc_results);

                            if (rc_results.Count > 0)
                                for (int i = 0; i < rc_results.Count; ++i)
                                {
                                    SguiWindow window = rc_results[i].gameObject.GetComponentInParent<SguiWindow>();
                                    if (window != null)
                                    {
                                        window.TakeFocus();
                                        return true;
                                    }
                                }
                        }
                        break;
                }
            return false;
        }
    }
}