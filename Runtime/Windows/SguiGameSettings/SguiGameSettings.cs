using _ARK_;

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

        protected override void Start()
        {
            base.Start();
            NUCLEOR.instance.subScheduler.AddRoutine(Util.EWhile(
                () => state_base == 0,
                null,
                () => sgui_toggle_window.Update(true))
                );
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}