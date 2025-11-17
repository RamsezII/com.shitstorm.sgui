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
            bool consumed = false;

            if (e.type == EventType.MouseDown)
                switch (e.keyCode)
                {
                    case KeyCode.Mouse0:
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
                                    consumed = true;
                                    break;
                                }
                            }

                            raycast_results.Clear();
                        }
                        break;
                }

            return consumed;
        }
    }
}