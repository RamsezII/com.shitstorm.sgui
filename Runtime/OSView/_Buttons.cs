using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace _SGUI_
{
    partial class OSView
    {
        public Button
            button_bottom_audio,
            button_home_settings,
            button_user_settings;

        readonly Dictionary<Type, OSButton> osbuttons_prefabs = new();

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