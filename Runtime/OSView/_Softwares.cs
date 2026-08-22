using System;

namespace _SGUI_
{
    partial class OSView
    {
        public static T InstantiateSoftware<T>() where T : SguiSoftware => (T)InstantiateSoftware(typeof(T));
        public static SguiSoftware InstantiateSoftware(in Type type) => InstantiateSoftware((SguiSoftware)Util.LoadResourceByType(type));
        public static SguiSoftware InstantiateSoftware(in SguiSoftware prefab)
        {
            instance.ToggleSelf(true);

            SguiSoftware clone = Instantiate(prefab, instance.rt_softwares);
            clone.Initialize();

            return clone;
        }
    }
}
