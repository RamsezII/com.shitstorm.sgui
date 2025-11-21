using _ARK_;
using System;

namespace _SGUI_
{
    partial class SguiCodium
    {
        [Serializable]
        public class Settings : UserJSon
        {
            public bool
                use_intellisense = true,
                space_confirms_completion = false;
        }

        public static Settings settings = new();

        //--------------------------------------------------------------------------------------------------------------

        static void LoadSettings(in bool log)
        {
            StaticJSon.ReadStaticJSon(out settings, true, log);
        }

        static void SaveSettings(in bool log)
        {
            settings.SaveStaticJSon(log);
        }
    }
}