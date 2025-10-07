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

        [HideInInspector]
        public SoftwareButton
            button_terminal_1,
            button_terminal_2,
            button_explorer,
            button_microship,
            button_codium,
            button_notepad,
            button_gallery,
            button_music,
            button_video;

        //--------------------------------------------------------------------------------------------------------------

        void AwakeButtons()
        {
            foreach (OSButton button in canvas2D.GetComponentsInChildren<OSButton>(true))
                osbuttons_prefabs[button.GetType()] = button;
            osbuttons_prefabs[typeof(SoftwareButton)] = canvas2D.transform.Find("_SGUI_.OSView/task-bar/buttons-left/" + typeof(SoftwareButton).FullName).GetComponent<SoftwareButton>();

            button_terminal_1 = canvas2D.transform.Find("_SGUI_.OSView/task-bar/buttons-left/cobra_terminal").GetComponent<SoftwareButton>();
            button_terminal_2 = canvas2D.transform.Find("_SGUI_.OSView/task-bar/buttons-left/harbinger_terminal").GetComponent<SoftwareButton>();
            button_explorer = canvas2D.transform.Find("_SGUI_.OSView/task-bar/buttons-left/explorer").GetComponent<SoftwareButton>();
            button_microship = canvas2D.transform.Find("_SGUI_.OSView/task-bar/buttons-left/microship").GetComponent<SoftwareButton>();
            button_codium = canvas2D.transform.Find("_SGUI_.OSView/task-bar/buttons-left/codium").GetComponent<SoftwareButton>();
            button_notepad = canvas2D.transform.Find("_SGUI_.OSView/task-bar/buttons-left/notepad").GetComponent<SoftwareButton>();
            button_gallery = canvas2D.transform.Find("_SGUI_.OSView/task-bar/buttons-left/gallery").GetComponent<SoftwareButton>();
            button_music = canvas2D.transform.Find("_SGUI_.OSView/task-bar/buttons-left/music").GetComponent<SoftwareButton>();
            button_video = canvas2D.transform.Find("_SGUI_.OSView/task-bar/buttons-left/video").GetComponent<SoftwareButton>();

            button_header_play = canvas2D.transform.Find("button-play").GetComponent<Button>();
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