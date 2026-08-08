using System;

namespace _SGUI_
{
    partial class OSView
    {
        public static T InstantiateSoftware<T>() where T : SguiWindow1 => (T)InstantiateSoftware(typeof(T));
        public static SguiWindow1 InstantiateSoftware(in Type type) => InstantiateSoftware((SguiWindow1)Util.LoadResourceByType(type));
        public static SguiWindow1 InstantiateSoftware(in SguiWindow1 prefab)
        {
            instance.ToggleSelf(true);

            SguiWindow1 clone = Instantiate(prefab, instance.rt_softwares);
            clone.OnAwake();

            return clone;
        }
    }
}