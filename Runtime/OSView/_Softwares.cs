using _SGUI_.composer;
using System;

namespace _SGUI_
{
    partial class OSView
    {
        public static T InstantiateSoftware<T>() where T : SguiFrame => (T)InstantiateSoftware(typeof(T));
        public static SguiFrame InstantiateSoftware(in Type type) => InstantiateSoftware((SguiFrame)Util.LoadResourceByType(type));
        public static SguiFrame InstantiateSoftware(in SguiFrame prefab) => InstantiateSoftware(prefab, false);

        public static SguiFrame InstantiateSoftware(SguiFrame prefab, bool new_window)
        {
            if (prefab == null) throw new ArgumentNullException(nameof(prefab));
            instance.ToggleSelf(true);
            SguiComposer composer = null;
            if (!new_window)
                foreach (var window in SguiWindow.instances.ReversedOrderIteration())
                    if (window.element is SguiComposer candidate && !candidate.oblivionized)
                    {
                        composer = candidate;
                        break;
                    }
            if (composer == null)
            {
                composer = Instantiate((SguiComposer)Util.LoadResourceByType(typeof(SguiComposer)), instance.rt_softwares);
                composer.Initialize();
                composer.rt.sizeDelta = ((UnityEngine.RectTransform)prefab.transform).sizeDelta;
            }
            composer.TakeFocus();
            var view = composer.active_splitview ?? composer.AddSplitView();
            var frame = view.AddTab(prefab);
            frame.TakeFocus();
            return frame;
        }
    }
}
