namespace _SGUI_
{
    public class SguiGameSettings : SguiWindow
    {
        public static SguiGameSettings instance;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            base.Awake();
            instance = this;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}