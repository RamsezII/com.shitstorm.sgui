using _ARK_;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_.Explorer
{
    internal partial class Button_Hierarchy : ArkComponent, IPointerClickHandler, SguiDragManager.IDraggable
    {
        public SguiExplorerView view;

        [SerializeField] RawImage rimg_selected;
        public RectTransform rt;
        public TextMeshProUGUI text;
        public int depth;

        public FileSystemInfo current_fsi;
        public string normalized_path;

        string SguiDragManager.IDraggable.DragDisplay => normalized_path;
        object SguiDragManager.IDraggable.DragData => normalized_path;

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

            normalized_path = current_fsi.FullName.NormalizePath();

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
                    var trad_rename = new Traductions()
                    {
                        french = $"Renommer",
                        english = $"Rename",
                    };

                    button.trad.SetTrads(trad_rename);

                    button.button.onClick.AddListener(() =>
                    {
                        var window = SguiWindow.InstantiateWindow<SguiCustom>();
                        window.trad_title.SetTrads(trad_rename);

                        var inputfield = window.AddButton<SguiCustom_InputField>();
                        inputfield.trad_label.SetTrads(new()
                        {
                            french = "Nouveau nom :",
                            english = "New name:",
                        });

                        window.onFunc_confirm += () =>
                        {
                            string name = inputfield.input_field.text;

                            if (string.IsNullOrWhiteSpace(name))
                            {
                                SguiWindow.ShowAlert(SguiDialogs.Error, out _, new()
                                {
                                    french = "Choisissez un nom",
                                    english = "Choose a name",
                                });
                                return false;
                            }
                            else
                            {
                                if (this == view.selected_fsi._value)
                                    view.selected_fsi.Value = null;

                                var pdir = Directory.GetParent(current_fsi.FullName);
                                string newpath = Path.Combine(pdir.FullName, name);

                                switch (current_fsi)
                                {
                                    case FileInfo file:
                                        file.MoveTo(newpath);
                                        break;

                                    case DirectoryInfo dir:
                                        dir.MoveTo(newpath);
                                        break;
                                }

                                view.RebuildHierarchy();
                                view.selected_fsi.Value = this;

                                return true;
                            }
                        };
                    });
                }

                {
                    var button = list.AddButton();
                    var trad_delete = new Traductions()
                    {
                        french = $"Supprimer",
                        english = $"Delete",
                    };

                    button.trad.SetTrads(trad_delete);

                    button.button.onClick.AddListener(() =>
                    {
                        var window = SguiWindow.ShowAlert(SguiDialogs.Dialog, out _, new()
                        {
                            french = $"Supprimer \"{current_fsi.Name}\" ?",
                            english = $"Delete \"{current_fsi.Name}\"?",
                        });

                        window.onAction_confirm += () =>
                        {
                            if (this == view.selected_fsi._value)
                                view.selected_fsi.Value = null;

                            switch (current_fsi)
                            {
                                case FileInfo file:
                                    file.Delete();
                                    break;

                                case DirectoryInfo dir:
                                    dir.Delete(true);
                                    break;
                            }

                            view.RebuildHierarchy();
                        };
                    });
                }

                {
                    var button = list.AddButton();
                    button.trad.SetTrads(new()
                    {
                        french = "Copier le chemin",
                        english = "Copy path",
                    });

                    button.button.onClick.AddListener(() =>
                    {
                        string path = current_fsi.FullName.NormalizePath();
                        GUIUtility.systemCopyBuffer = path;
                        LoggerOverlay.Log($"Path copied to clipboard ({path})", this, timer: 5);
                    });
                }

                {
                    var button = list.AddButton();
                    button.trad.SetTrads(new()
                    {
                        french = "Ouvrir dans l'explorateur",
                        english = "Open in the explorer",
                    });
                    button.button.onClick.AddListener(() => Application.OpenURL(current_fsi.FullName));
                }

                OnContextList(list);
            }
        }

        protected virtual void OnContextList(in SguiContextClick_List list)
        {
        }

        void SguiDragManager.IDraggable.OnDropAccepted(in SguiDragManager.IAcceptDraggable acceptor)
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