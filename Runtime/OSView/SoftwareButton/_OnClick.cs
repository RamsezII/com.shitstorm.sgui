using _UTIL_;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_
{
    partial class SoftwareButton : IPointerClickHandler
    {
        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (software_prefab == null)
            {
                LoggerOverlay.Log($"{nameof(software_prefab)} is null ({software_prefab})", this, logLevel: LoggerOverlay.LogLevel.Warning);
                return;
            }

            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    if (software_instances.IsEmpty)
                    {
                        if (onClick_left_empty != null)
                            onClick_left_empty(eventData);
                        else
                            InstantiateSoftware();
                    }
                    else
                    {
                        if (onClick_left_notEmpty == null || onClick_left_notEmpty(eventData))
                            if (software_instances._collection.Count == 1)
                            {
                                SguiWindow instance = software_instances._collection[0];
                                instance.SetScalePivot(this);

                                if (instance.HasFocus())
                                    instance.ToggleWindow();
                                else
                                    instance.TakeFocus();
                            }
                    }
                    break;

                case PointerEventData.InputButton.Middle:
                    onClick_middle?.Invoke(eventData);
                    break;

                case PointerEventData.InputButton.Right:
                    if (onClick_right == null || onClick_right(eventData))
                        if (dropdownOptions._collection.Count > 0)
                        {
                            dropdown.Show();
                            var toggles = dropdown.GetComponentsInChildren<Toggle>(false);
                            for (int i = 0; i < toggles.Length; ++i)
                            {
                                var click_handler = toggles[i].GetComponent<PointerClickHandler>();
                                var (trad, action) = dropdownOptions._collection[i];
                                click_handler.onClick = (PointerEventData eventData) => action();
                            }
                        }
                    break;
            }
        }
    }
}