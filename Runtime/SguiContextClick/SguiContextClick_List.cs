using _ARK_;
using _UTIL_;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public sealed class SguiContextClick_List : MonoBehaviour
    {
        public RectTransform list_rt, content_rt;
        public ScrollRect scrollview;
        public VerticalLayoutGroup layout;
        [SerializeField] SguiContextClick_List_Button prefab_button;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            scrollview = GetComponentInChildren<ScrollRect>();
            list_rt = (RectTransform)scrollview.transform.parent;
            layout = GetComponentInChildren<VerticalLayoutGroup>();
            content_rt = (RectTransform)layout.transform.parent;
            prefab_button = GetComponentInChildren<SguiContextClick_List_Button>();

            GetComponentInChildren<OnPointerClick>().onClick += eventData => Destroy(gameObject);
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            prefab_button.gameObject.SetActive(false);
            AutoSizeAndMove();
        }

        //--------------------------------------------------------------------------------------------------------------

        public SguiContextClick_List_Button AddButton()
        {
            var clone = Instantiate(prefab_button, prefab_button.transform.parent);

            clone.gameObject.SetActive(true);
            clone.button.onClick.AddListener(() => Destroy(gameObject));

            if (didStart)
                AutoSizeAndMove();

            return clone;
        }

        public void AutoSizeAndMove()
        {
            if (gameObject == null)
                return;

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)layout.transform);

            content_rt.sizeDelta = new(0, layout.preferredHeight);
            list_rt.sizeDelta = new Vector2(list_rt.sizeDelta.x, Mathf.Min(300, layout.preferredHeight));
        }
    }
}