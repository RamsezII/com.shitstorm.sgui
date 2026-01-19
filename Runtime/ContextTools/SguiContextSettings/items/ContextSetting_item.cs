using _ARK_;
using _SGUI_.context_hover;

namespace _SGUI_.context_tools.settings
{
    public abstract class ContextSetting_item : ArkComponent
    {
        public SguiContextSettings settings;

        public ContextHoverHandler hover_handler;
        public Traductable trad;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            settings = GetComponentInParent<SguiContextSettings>(true);

            hover_handler = GetComponentInChildren<ContextHoverHandler>(true);
            trad = GetComponentInChildren<Traductable>(true);

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