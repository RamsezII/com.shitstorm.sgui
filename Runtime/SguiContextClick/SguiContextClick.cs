using TMPro;
using UnityEngine;

namespace _SGUI_
{
    public class SguiContextClick : MonoBehaviour
    {
        public static SguiContextClick instance;

        RectTransform pos_rt;
        TMP_Dropdown dropdown;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;

            pos_rt = (RectTransform)transform.Find("position");
            dropdown = transform.Find("position/dropdown").GetComponent<TMP_Dropdown>();
        }

        //--------------------------------------------------------------------------------------------------------------

        public void RightClickHere(in Vector2 mousePosition)
        {
            pos_rt.position = mousePosition;
            dropdown.Show();
        }
    }
}