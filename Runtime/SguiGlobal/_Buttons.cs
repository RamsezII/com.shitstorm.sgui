using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace _SGUI_
{
    partial class SguiGlobal
    {
        readonly Dictionary<Type, OSButton> osbuttons_prefabs = new();

        public Button button_bottom_audio;

        //--------------------------------------------------------------------------------------------------------------

        void AwakeButtons()
        {
            foreach (OSButton button in canvas2D.GetComponentsInChildren<OSButton>(true))
                osbuttons_prefabs[button.GetType()] = button;
            osbuttons_prefabs[typeof(SoftwareButton)] = canvas2D.transform.Find("_SGUI_.OSView/task-bar/buttons-left/" + typeof(SoftwareButton).FullName).GetComponent<SoftwareButton>();

            button_bottom_audio = canvas2D.transform.Find("_SGUI_.OSView/task-bar/buttons-right/audio/button").GetComponent<Button>();
        }

        //--------------------------------------------------------------------------------------------------------------

        void StartButtons()
        {
            foreach (var pair in osbuttons_prefabs)
                pair.Value.gameObject.SetActive(false);
        }

        //--------------------------------------------------------------------------------------------------------------

        public T AddButton<T>() where T : OSButton
        {
            if (osbuttons_prefabs.TryGetValue(typeof(T), out var prefab))
            {
                T button = (T)Instantiate(prefab, prefab.transform.parent);
                button.gameObject.SetActive(true);
                return button;
            }
            else
                throw new ArgumentException($"no button of type: {typeof(T)}");
        }
    }
}