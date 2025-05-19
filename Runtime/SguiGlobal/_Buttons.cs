using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    partial class SguiGlobal
    {
        readonly Dictionary<Type, OSButton> osbuttons_prefabs = new();

        public Button button_header_play, button_bottom_audio;

        [HideInInspector] public TaskbarButton_left button_terminal;

        //--------------------------------------------------------------------------------------------------------------

        void AwakeButtons()
        {
            foreach (OSButton button in canvas2D.GetComponentsInChildren<OSButton>(true))
                osbuttons_prefabs[button.GetType()] = button;
            osbuttons_prefabs[typeof(TaskbarButton_left)] = canvas2D.transform.Find("task-bar/buttons-left/taskbar-button-left").GetComponent<TaskbarButton_left>();

            button_terminal = canvas2D.transform.Find("task-bar/buttons-left/terminal").GetComponent<TaskbarButton_left>();

            button_header_play = canvas2D.transform.Find("button-play").GetComponent<Button>();
            button_bottom_audio = canvas2D.transform.Find("task-bar/buttons-right/audio/button").GetComponent<Button>();
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