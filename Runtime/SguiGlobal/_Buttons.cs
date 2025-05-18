using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    partial class SguiGlobal
    {
        readonly Dictionary<Type, OSButton> osbuttons_prefabs = new();

        public Button button_footer_main, button_header_play, button_bottom_audio;

        //--------------------------------------------------------------------------------------------------------------

        void AwakeButtons()
        {
            RectTransform buttons_rT = (RectTransform)canvas2D.transform.Find("header/buttons-left");
            for (int i = 0; i < buttons_rT.childCount; ++i)
                if (buttons_rT.GetChild(i).TryGetComponent<OSButton>(out var button))
                    osbuttons_prefabs.Add(button.GetType(), button);

            button_header_play = canvas2D.transform.Find("button-play").GetComponent<Button>();
            button_bottom_audio = canvas2D.transform.Find("task-bar/buttons-right/audio/button").GetComponent<Button>();
            button_footer_main = canvas2D.transform.Find("task-bar/main-button").GetComponent<Button>();
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