using _ARK_;
using UnityEngine;

namespace _SGUI_
{
    public sealed class SguiContextHandlerHover : MonoBehaviour, SguiContextHover.IUser
    {
        public Traductions hover_infos;
        Traductions SguiContextHover.IUser.OnSguiContextHover() => hover_infos;
    }
}