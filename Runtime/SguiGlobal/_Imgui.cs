using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    partial class SguiGlobal
    {
        readonly List<RaycastResult> raycast_results = new();

        //--------------------------------------------------------------------------------------------------------------

        bool OnImguiInputs(Event e)
        {
            if (e.type == EventType.MouseDown)
                if (e.keyCode == KeyCode.Mouse0)
                {
                    raycast_results.Clear();

                    PointerEventData data = new(EventSystem.current)
                    {
                        position = Input.mousePosition
                    };

                    raycaster_2D.Raycast(data, raycast_results);

                    if (raycast_results.Count > 0)
                    {
                        for (int i = 0; i < raycast_results.Count; ++i)
                        {
                            SguiWindow window = raycast_results[i].gameObject.GetComponentInParent<SguiWindow>();
                            if (window != null)
                            {
                                window.transform.SetAsLastSibling();
                                break;
                            }
                        }

                        raycast_results.Clear();
                    }
                }
            return false;
        }
    }
}