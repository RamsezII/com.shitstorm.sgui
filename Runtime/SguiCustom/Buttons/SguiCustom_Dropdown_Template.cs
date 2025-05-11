using UnityEngine;

namespace _SGUI_
{
    public class SguiCustom_Dropdown_Template : MonoBehaviour
    {
        private void Start() => GetComponentInParent<SguiCustom_Dropdown>().on_template_clone?.Invoke(this);
    }
}