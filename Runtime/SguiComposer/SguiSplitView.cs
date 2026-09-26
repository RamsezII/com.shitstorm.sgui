using _ARK_;
using _UTIL_;
using System;
using UnityEngine;

namespace _SGUI_.composer
{
    internal sealed partial class SguiSplitView : ArkComponent1
    {
        public readonly ValueNotifier<SguiTabHeader> current_tab = new();

        [SerializeField] RectTransform rt_tabs, rt_body;
        [SerializeField] internal ContextListHandler listhandler;

        //--------------------------------------------------------------------------------------------------------------

        public T AddTab<T>() where T : SguiFrame => (T)AddTab(typeof(T));
        public SguiFrame AddTab(in Type type)
        {
            current_tab.Value = Util.InstantiateOrCreate<SguiTabHeader>(parent: rt_tabs);

            var frame = (SguiFrame)Util.InstantiateOrCreate(type, parent: rt_body);
            frame.tab = current_tab._value;

            current_tab._value.isSelected.AddListener(frame.gameObject.SetActive);

            return frame;
        }
    }
}