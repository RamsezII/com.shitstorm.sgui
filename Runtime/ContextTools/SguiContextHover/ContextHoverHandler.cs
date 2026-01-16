using _ARK_;
using UnityEngine;

namespace _SGUI_.context_hover
{
    public sealed class ContextHoverHandler : MonoBehaviour, SguiContextHover.IUser
    {
        public Traductions hover_infos;
        Traductions SguiContextHover.IUser.OnSguiContextHover() => hover_infos;
    }
}