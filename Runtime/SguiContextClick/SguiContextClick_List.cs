using _ARK_;
using _UTIL_;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public sealed class SguiContextClick_List : MonoBehaviour
    {
        public RectTransform rt;
        public ScrollRect scrollview;
        public VerticalLayoutGroup layout;
        [SerializeField] SguiContextClick_List_Button prefab_button;
        Vector2 init_size;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            scrollview = GetComponentInChildren<ScrollRect>();
            rt = (RectTransform)scrollview.transform.parent;
            layout = GetComponentInChildren<VerticalLayoutGroup>();
            prefab_button = GetComponentInChildren<SguiContextClick_List_Button>();

            GetComponentInChildren<OnPointerClick>().onClick += eventData => Destroy(gameObject);

            init_size = rt.sizeDelta;
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
                AutoSizeAndMove_delayed();

            return clone;
        }

        public void AutoSizeAndMove_delayed() => Util.AddAction(ref NUCLEOR.delegates.Update_OnStartOfFrame_once, AutoSizeAndMove);
        public void AutoSizeAndMove()
        {
            if (gameObject == null)
                return;

            rt.sizeDelta = new Vector2(init_size.x, layout.preferredHeight + layout.padding.top + layout.padding.bottom);
        }
    }
}