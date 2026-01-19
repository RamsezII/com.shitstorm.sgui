using _ARK_;
using _SGUI_.context_hover;

namespace _SGUI_.context_tools.settings
{
    public abstract class ContextSetting_item : ArkComponent, SguiContextHover.IUser
    {
        public SguiContextSettings settings;

        public ContextHoverHandler hover_handler;
        public Traductable label_trad;
        public Traductions hover_infos;
        Traductions SguiContextHover.IUser.OnSguiContextHover() => hover_infos;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            settings = GetComponentInParent<SguiContextSettings>(true);

            hover_handler = GetComponentInChildren<ContextHoverHandler>(true);
            label_trad = GetComponentInChildren<Traductable>(true);

            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();
            settings.clones.Add(this);
        }
    }
}