using UnityEngine;

namespace _SGUI_
{
    public class SguiContextClick : MonoBehaviour
    {
        public static SguiContextClick instance;

        [SerializeField] SguiContextClick_List prefab_scrollview;
        [SerializeField] SguiContextClick_List scrollview_lastInstance;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;
            prefab_scrollview = GetComponentInChildren<SguiContextClick_List>();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            prefab_scrollview.gameObject.SetActive(false);
        }

        //--------------------------------------------------------------------------------------------------------------

        public SguiContextClick_List RightClickHere(in Vector2 mousePosition, in bool replace)
        {
            if (replace)
                if (scrollview_lastInstance != null)
                    Destroy(scrollview_lastInstance.gameObject);

            var clone = scrollview_lastInstance = Instantiate(prefab_scrollview, prefab_scrollview.transform.parent);

            clone.gameObject.SetActive(true);
            clone.list_rt.position = mousePosition;

            return clone;
        }
    }
}