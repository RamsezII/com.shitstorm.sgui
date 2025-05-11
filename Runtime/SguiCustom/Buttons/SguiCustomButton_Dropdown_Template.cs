using UnityEngine;

namespace _SGUI_
{
    public class SguiCustomButton_Dropdown_Template : MonoBehaviour
    {
        private void Start() => GetComponentInParent<SguiCustomButton_Dropdown>().on_template_clone?.Invoke(this);
    }
}