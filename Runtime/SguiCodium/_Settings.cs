using _ARK_;
using System;

namespace _SGUI_
{
    partial class SguiCodium
    {
        [Serializable]
        public class Settings : UserJSon
        {
            public bool space_confirms_completion;
        }

        public static Settings settings = new();

        //--------------------------------------------------------------------------------------------------------------

        static void LoadSettings(in bool log)
        {
            StaticJSon.ReadStaticJSon(ref settings, true, log);
        }

        static void SaveSettings(in bool log)
        {
            settings.SaveStaticJSon(log);
        }
    }
}