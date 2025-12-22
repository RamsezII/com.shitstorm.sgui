using _ARK_;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_.Explorer
{
    internal class Button_Hierarchy : ArkComponent, IPointerClickHandler
    {
        public SguiExplorerView view;

        [SerializeField] RawImage rimg_selected;
        public RectTransform rt;
        public string full_path, short_path;
        public int depth;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            view = GetComponentInParent<SguiExplorerView>(true);
            rimg_selected = transform.Find("selected").GetComponent<RawImage>();
            rt = (RectTransform)transform.Find("rt");

            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            view.selected_line.AddListener(value =>
            {
                OnSelectedButton(value);
            });
        }

        //--------------------------------------------------------------------------------------------------------------

        void OnSelectedButton(Button_Hierarchy value)
        {
            bool selected = this == value;
            OnSelected(selected);
        }

        protected virtual void OnSelected(in bool selected)
        {
            rimg_selected.gameObject.SetActive(selected);
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            view.selected_line.Value = this;

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                var list = SguiContextClick.instance.InstantiateListHere(eventData.position);

                {
                    var button = list.AddButton();
                    button.trad.SetTrads(new()
                    {
                        french = "Renommer",
                        english = "Rename",
                    });
                }

                {
                    var button = list.AddButton();
                    button.trad.SetTrads(new()
                    {
                        french = "Supprimer",
                        english = "Delete",
                    });
                }

                list.AddLine();

                OnContextList(list);
            }
        }

        protected virtual void OnContextList(in SguiContextClick_List list)
        {

        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void OnDestroy()
        {
            base.OnDestroy();

            view.selected_line.RemoveListener(OnSelectedButton);
        }
    }
}