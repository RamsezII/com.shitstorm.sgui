using System;
using UnityEngine;

namespace _SGUI_
{
    [Obsolete]
    internal sealed class SguiCustom_Dropdown_Template : MonoBehaviour
    {
        private void Start() => GetComponentInParent<SguiCustom_Dropdown>().OnTemplateClone(this);
    }
}