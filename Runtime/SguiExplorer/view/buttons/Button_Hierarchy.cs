using _ARK_;
using _SGUI_.context_click;
using System;
using System.IO;
using TMPro;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_.Explorer
{
    internal partial class Button_Hierarchy : ArkComponent1, IPointerClickHandler, SguiDragManager.IDraggable
    {
        public SguiExplorerView view;
        [SerializeField] RectTransform rt_size;
        [SerializeField] Graphic graphic_selected;
        [SerializeField] internal Button button;
        [SerializeField] internal TextMeshProUGUI text;
        [SerializeField] internal int depth;

        [NonSerialized] public FileSystemInfo current_fsi;

        string SguiDragManager.IDraggable.DragDisplay => current_fsi.FullName;
        object SguiDragManager.IDraggable.DragData => current_fsi;

        [NoAutoStaticsCleanup] static IA_ButtonHierarchy iam;

        //--------------------------------------------------------------------------------------------------------------

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void InitInputs()
        {
            iam?.Disable();
            iam?.Dispose();
            iam = new();
            iam.Enable();

            iam.Main.Rename.performed += context =>
            {
                var ob = EventSystem.current.currentSelectedGameObject;
                if (ob != null)
                    if (ob.TryGetComponent<Button_Hierarchy>(out var instance))
                        instance.Rename();
            };
        }

        //--------------------------------------------------------------------------------------------------------------

        protected void Initialize(in FileSystemInfo fsi)
        {
            view = GetComponentInParent<SguiExplorerView>(true);
            current_fsi = fsi;
            text.text = fsi.Name;
            rt_size.sizeDelta = new Vector2(-5 * depth, rt_size.sizeDelta.y);
        }

        //--------------------------------------------------------------------------------------------------------------

        internal void SetSelected(bool selected) => graphic_selected.gameObject.SetActive(selected);

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            view.selected_fsi.Value = current_fsi;

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                var list = SguiContextList.instance.InstantiateListAtScreenPoint(eventData.position, eventData.pressEventCamera);

                {
                    var button = list.AddButton_trad(new()
                    {
                        french = $"Renommer",
                        english = $"Rename",
                    });
                    button.action += Rename;
                }

                {
                    var button = list.AddButton_trad(new()
                    {
                        french = $"Supprimer",
                        english = $"Delete",
                    });

                    button.action += () =>
                    {
                        var window = SguiCustom.ShowAlert(SguiDialogs.Dialog, out _, new()
                        {
                            french = $"Supprimer \"{current_fsi.Name}\" ?",
                            english = $"Delete \"{current_fsi.Name}\"?",
                        });

                        window.onAction_confirm += () =>
                        {
                            if (current_fsi == view.selected_fsi._value)
                                view.selected_fsi.Value = null;

                            switch (current_fsi)
                            {
                                case FileInfo file:
                                    file.Delete();
                                    break;

                                case DirectoryInfo dir:
                                    dir.Delete(recursive: true);
                                    break;
                            }

                            view.RebuildHierarchy();
                        };
                    };
                }

                {
                    var button = list.AddButton_trad(new()
                    {
                        french = "Copier le chemin",
                        english = "Copy path",
                    });

                    button.action += () =>
                    {
                        string path = current_fsi.FullName;
                        GUIUtility.systemCopyBuffer = path;
                        SguiLoggerOverlay.Log($"Path copied to clipboard ({path})", this, timer: 5);
                    };
                }

                {
                    var button = list.AddButton_trad(new()
                    {
                        french = "Ouvrir dans l'explorateur",
                        english = "Open in the explorer",
                    });
                    button.action += () => Application.OpenURL(current_fsi.FullName);
                }

                OnContextList(list);

                if (SguiExplorerView.onContextList_fileAndDirectory != null)
                {
                    list.AddLine();
                    SguiExplorerView.onContextList_fileAndDirectory(view, list, current_fsi);
                }
            }
        }

        protected virtual void OnContextList(in ContextList list)
        {
        }

        public void Rename()
        {
            SguiInputField.instance.ShowHere(text, placeholder: new(text.text), onSubmit: output =>
            {
                if (current_fsi == view.selected_fsi._value)
                    view.selected_fsi.Value = null;

                var pdir = Directory.GetParent(current_fsi.FullName);
                string newpath = Path.Combine(pdir.FullName, output);

                switch (current_fsi)
                {
                    case FileInfo file:
                        file.MoveTo(newpath);
                        view.selected_fsi.Value = file;
                        break;

                    case DirectoryInfo dir:
                        dir.MoveTo(newpath);
                        view.selected_fsi.Value = dir;
                        break;
                }

                view.RebuildHierarchy();
            });
        }

        public void Rename_old()
        {
            var window = SguiCustom.CreatePrompt();
            window.trad_title.SetTraductions(new() { french = "Renommer", english = "Rename", });

            var inputfield = window.AddButton<SguiCustom_InputField>();
            inputfield.trad_label.SetTraductions(new()
            {
                french = "Nouveau nom :",
                english = "New name:",
            });

            window.onFunc_confirm += () =>
            {
                string name = inputfield.input_field.text;

                if (string.IsNullOrWhiteSpace(name))
                {
                    SguiCustom.ShowAlert(SguiDialogs.Error, out _, new()
                    {
                        french = "Choisissez un nom",
                        english = "Choose a name",
                    });
                    return false;
                }
                else
                {
                    if (current_fsi == view.selected_fsi._value)
                        view.selected_fsi.Value = null;

                    var pdir = Directory.GetParent(current_fsi.FullName);
                    string newpath = Path.Combine(pdir.FullName, name);

                    switch (current_fsi)
                    {
                        case FileInfo file:
                            file.MoveTo(newpath);
                            view.selected_fsi.Value = file;
                            break;

                        case DirectoryInfo dir:
                            dir.MoveTo(newpath);
                            view.selected_fsi.Value = dir;
                            break;
                    }

                    view.RebuildHierarchy();
                    return true;
                }
            };
        }
    }
}
