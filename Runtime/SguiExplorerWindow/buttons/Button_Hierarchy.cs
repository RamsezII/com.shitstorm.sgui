using _ARK_;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_.Explorer
{
    internal class Button_Hierarchy : ArkComponent, IPointerClickHandler
    {
        public SguiExplorerView view;

        public RectTransform rt;
        public string full_path, short_path;
        public int depth;

        //--------------------------------------------------------------------------------------------------------------

        protected override void Awake()
        {
            view = GetComponentInParent<SguiExplorerView>(true);
            rt = (RectTransform)transform.Find("rt");

            base.Awake();
        }

        //--------------------------------------------------------------------------------------------------------------

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                var list = SguiContextClick.instance.InstantiateListHere(eventData.position);

                {
                    var button = list.AddButton();
                    button.trad.SetTrads(new()
                    {
                        french = $"Renommer \"{short_path}\"",
                        english = $"Rename \"{short_path}\"",
                    });
                }

                {
                    var button = list.AddButton();
                    button.trad.SetTrads(new()
                    {
                        french = $"Supprimer \"{short_path}\"",
                        english = $"Delete \"{short_path}\"",
                    });
                }

                list.AddLine();

                OnContextList(list);
            }
        }

        protected virtual void OnContextList(in SguiContextClick_List list)
        {

        }
    }
}