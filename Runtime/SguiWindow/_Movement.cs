using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    partial class SguiWindow
    {
        Vector2 saved_size;
        bool occupying_screen_portion;

        //--------------------------------------------------------------------------------------------------------------

        protected virtual void OnHeaderBeginDrag(PointerEventData eventData)
        {
            if (occupying_screen_portion)
            {
                occupying_screen_portion = false;
                Vector2 pos = rt.position;
                rt.anchorMin = rt.anchorMax = .5f * Vector2.one;
                rt.sizeDelta = saved_size;
                rt.position = pos;
            }
        }

        protected virtual void OnHeaderDrag(PointerEventData eventData)
        {
            if (this is SguiWindow1 window1 && window1.fullscreen._value)
                return;

            Vector2 delta = eventData.delta;
            rt.position += (Vector3)delta;

            if (!CheckPosition(out Vector2 correc))
                ResizerVisual.instance.UntakeFocus(this);
            else
            {
                bool needs_visual = true;

                RectTransform rt_resizer = ResizerVisual.instance.rt;

                // top
                if (correc.y < 0)
                    // top right
                    if (correc.x < 0)
                    {
                        rt_resizer.anchorMin = new Vector2(.5f, .5f);
                        rt_resizer.anchorMax = new Vector2(1, 1);
                    }
                    // top left
                    else if (correc.x > 0)
                    {
                        rt_resizer.anchorMin = new Vector2(0, .5f);
                        rt_resizer.anchorMax = new Vector2(.5f, 1);
                    }
                    // top
                    else
                    {
                        rt_resizer.anchorMin = new Vector2(0, .5f);
                        rt_resizer.anchorMax = new Vector2(1, 1);
                    }
                // bottom
                else if (correc.y > 0)
                    // bottom right
                    if (correc.x < 0)
                    {
                        rt_resizer.anchorMin = new Vector2(.5f, 0);
                        rt_resizer.anchorMax = new Vector2(1, .5f);
                    }
                    // bottom left
                    else if (correc.x > 0)
                    {
                        rt_resizer.anchorMin = new Vector2(0, 0);
                        rt_resizer.anchorMax = new Vector2(.5f, .5f);
                    }
                    // bottom
                    else
                    {
                        rt_resizer.anchorMin = new Vector2(0, 0);
                        rt_resizer.anchorMax = new Vector2(1, .5f);
                    }
                // right
                else if (correc.x < 0)
                {
                    rt_resizer.anchorMin = new Vector2(.5f, 0);
                    rt_resizer.anchorMax = new Vector2(1, 1);
                }
                // left
                else if (correc.x > 0)
                {
                    rt_resizer.anchorMin = new Vector2(0, 0);
                    rt_resizer.anchorMax = new Vector2(.5f, 1);
                }
                else
                    needs_visual = false;

                if (needs_visual)
                {
                    ResizerVisual.instance.TakeFocus(this);
                    rt_resizer.sizeDelta = -10 * Vector2.one;
                    rt_resizer.anchoredPosition = Vector2.zero;
                }
                else
                {
                    LoggerOverlay.Log($"did not need visual", this, logLevel: LoggerOverlay.LogLevel.Warning);
                    ResizerVisual.instance.UntakeFocus(this);
                }
            }
        }

        protected virtual void OnHeaderEndDrag(PointerEventData eventData)
        {
            if (ResizerVisual.instance.UntakeFocus(this))
            {
                occupying_screen_portion = true;
                saved_size = rt.rect.size;

                rt.sizeDelta = Vector2.zero;
                rt.anchoredPosition = Vector2.zero;
                rt.anchorMin = ResizerVisual.instance.rt.anchorMin;
                rt.anchorMax = ResizerVisual.instance.rt.anchorMax;
            }
        }

        public bool CheckPosition(out Vector2 correction)
        {
            rt.GetWorldCorners(out Vector2 min, out Vector2 max);
            rt_parent.GetWorldCorners(out Vector2 p_min, out Vector2 p_max);

            if (Util.BoundsClamp(min, max, p_min, p_max, out correction))
            {
                rt.position += (Vector3)correction;
                return true;
            }

            correction = Vector2.zero;
            return false;
        }
    }
}