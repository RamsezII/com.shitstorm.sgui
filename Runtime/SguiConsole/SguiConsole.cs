using _SGUI_.composer;
using _UTIL_;
using System.Collections.Generic;
using TMPro;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public sealed partial class SguiConsole : SguiFrame
    {
        [AutoStaticsCleanup] static new readonly HashSet<SguiConsole> instances = new();

        [SerializeField] TMP_InputField tmp_selected;
        [SerializeField] ScrollRect scrollview_list, scrollview_selected;
        [SerializeField] ScrollHandler scroll_selected;
        [SerializeField] internal Texture icon_log, icon_warn, icon_err;

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnInitialize()
        {
            base.OnInitialize();

            tab.trad_title.SetText("Console");

            tmp_selected.text = string.Empty;

            var button_clear = header.AddButton(new() { french = "Vider", english = "Clear", });
            button_clear.button.onClick.AddListener(Clear);

            scroll_selected.onScroll += scrollview_selected.OnScroll;

            InitRatio();
            InitEntries();

            instances.Add(this);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDestroy()
        {
            instances.Remove(this);

            base.OnDestroy();
        }
    }
}
