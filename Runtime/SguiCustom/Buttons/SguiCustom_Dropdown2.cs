using _ARK_;
using _SGUI_.context_click;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public class SguiCustom_Dropdown2 : SguiCustom_Abstract
    {
        public Button _button;
        [SerializeField] Traductable trad_button;
        public SguiListTypes type;
        public Action<ContextList> onContextList;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            _button.onClick.AddListener(() =>
            {
                var list = SguiContextList.instance.InstantiateListHere(rT.position);
                list.type = type;
                list.target_trad = trad_button;
                onContextList(list);
            });
        }
    }
}