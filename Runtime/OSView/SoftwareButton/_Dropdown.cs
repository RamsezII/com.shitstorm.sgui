using _ARK_;
using _UTIL_;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    partial class SoftwareButton
    {
        TMP_Dropdown dropdown;

        public Action<PointerEventData> onClick_left_empty, onClick_middle;
        public Func<PointerEventData, bool> onClick_left_notEmpty, onClick_right;

        public readonly ListListener<(Traductions trad, Action action)> dropdownOptions = new();

        //--------------------------------------------------------------------------------------------------------------

        void StartDropdown()
        {
            software_instances.AddListener2(list =>
            {
                for (int i = 0; i < img_instances.Length; i++)
                    img_instances[i].gameObject.SetActive(list.Count > i);

                dropdownOptions.Modify(_list =>
                {
                    _list.Clear();

                    _list.Add((
                        trad: new Traductions()
                        {
                            french = $"Ajouter une fenêtre",
                            english = $"Add a new window",
                        },
                        action: () =>
                        {
                            InstantiateSoftware();
                        }
                    ));

                    for (int i = 0; i < software_instances._collection.Count; i++)
                    {
                        SguiWindow window = software_instances._collection[i];
                        if (window.trad_title == null)
                            LoggerOverlay.Log($"error trad: {window}", window, logLevel: LoggerOverlay.LogLevel.Warning);
                        else
                            _list.Add((
                                trad: window.sgui_description._value,
                                action: () =>
                                {
                                    window.SetScalePivot(this);
                                    window.ToggleWindow(true);
                                    window.TakeFocus();
                                }
                            ));
                    }

                    _list.Add((
                        trad: new Traductions()
                        {
                            french = $"Fermer toutes les fenêtres",
                            english = $"Close all windows",
                        },
                        action: () =>
                        {
                            for (int i = software_instances._collection.Count - 1; i >= 0; --i)
                            {
                                SguiWindow window = software_instances._collection[i];
                                window.SetScalePivot(null);
                                window.Oblivionize();
                            }
                        }
                    ));
                });

                RefreshOpenState();
            });

            dropdownOptions.AddListener2(_list =>
            {
                dropdown.ClearOptions();
                List<string> options = _list.Select(x => x.trad.Automatic).ToList();
                dropdown.AddOptions(options);
            });
        }
    }
}