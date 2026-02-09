using _UTIL_;
using UnityEngine;

namespace _SGUI_.prompts.color_prompt
{
    [RequireComponent(typeof(CanvasRenderer))]
    internal class DiskRaycastable : UI_EmptyGraphic
    {
        [SerializeField] RectTransform rt;
        [SerializeField] float min = .5f, max = 1;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            rt = (RectTransform)transform;
            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        public override bool Raycast(Vector2 sp, Camera eventCamera)
        {
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, sp, eventCamera, out Vector2 localPoint))
                return false;
            float radius = 2 * (localPoint / rt.rect.size).magnitude;
            return radius >= min && radius <= max;
        }
    }
}