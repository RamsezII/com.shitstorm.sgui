using _ARK_;
using System.IO;
using TMPro;
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
        public TextMeshProUGUI text;
        public int depth;

        public FileSystemInfo current_fsi;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            view = GetComponentInParent<SguiExplorerView>(true);
            rimg_selected = transform.Find("selected").GetComponent<RawImage>();
            rt = (RectTransform)transform.Find("rt");
            text = rt.Find("text").GetComponent<TextMeshProUGUI>();

            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override void Start()
        {
            base.Start();

            view.selected_fsi.AddListener(OnSelectedButton);

            rt.anchoredPosition += new Vector2(5 * depth, 0);
        }

        //--------------------------------------------------------------------------------------------------------------

        internal virtual void AssignFsi(in FileSystemInfo fsi)
        {
            current_fsi = fsi;
            text.text = fsi.Name;
        }

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
            view.selected_fsi.Value = this;

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

            view.selected_fsi.RemoveListener(OnSelectedButton);
        }
    }
}