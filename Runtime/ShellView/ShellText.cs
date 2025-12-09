using _UTIL_;
using UnityEngine;

namespace _SGUI_
{
    public sealed class ShellText : TMPro.TextMeshProUGUI
    {
        //public float WordWrappingRatios { get => _show_wordWrappingRatios; set { wordWrappingRatios = value; } }
        //[ShowProperty(nameof(WordWrappingRatios)), Range(0, 1)] public float _show_wordWrappingRatios;

        //----------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            base.Awake();
        }

        //----------------------------------------------------------------------------------------------------------

        [ContextMenu(nameof(WWR_0))]
        void WWR_0()
        {
            wordWrappingRatios = 0;
        }

        [ContextMenu(nameof(WWR_1))]
        void WWR_1()
        {
            wordWrappingRatios = 1;
        }
    }
}