using _ARK_;

namespace _SGUI_
{
    partial class SoftwareButton : SguiContextHover.IUser
    {
        public Traductions hover_info;

        //--------------------------------------------------------------------------------------------------------------

        Traductions SguiContextHover.IUser.OnSguiContextHover() => hover_info;
    }
}