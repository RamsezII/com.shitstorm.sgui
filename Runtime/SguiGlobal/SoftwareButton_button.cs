using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _SGUI_
{
    internal class SoftwareButton_button : Button
    {
        private bool leftPressed = false;
        private bool rightPressed = false;
        private bool middlePressed = false;

        public override void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("down: " + eventData.button, this);

            if (eventData.button == PointerEventData.InputButton.Left)
                leftPressed = true;
            else if (eventData.button == PointerEventData.InputButton.Right)
                rightPressed = true;
            else if (eventData.button == PointerEventData.InputButton.Middle)
                middlePressed = true;

            // Feedback visuel “pressed” forcé pour tous les boutons
            DoStateTransition(SelectionState.Pressed, false);

            // Appelle le comportement de base pour le clic gauche (si tu veux que le bouton reste compatible UnityEvent classique)
            if (eventData.button == PointerEventData.InputButton.Left)
                base.OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("up: " + eventData.button, this);

            if (eventData.button == PointerEventData.InputButton.Left)
                leftPressed = false;
            else if (eventData.button == PointerEventData.InputButton.Right)
                rightPressed = false;
            else if (eventData.button == PointerEventData.InputButton.Middle)
                middlePressed = false;

            // On sort de l’état pressed **uniquement si plus aucun bouton n’est maintenu**
            if (!leftPressed && !rightPressed && !middlePressed)
            {
                // Retour visuel selon la position de la souris
                bool isPointerInside = IsHighlighted();
                DoStateTransition(isPointerInside ? SelectionState.Highlighted : SelectionState.Normal, false);
            }

            // Comportement standard pour clic gauche
            if (eventData.button == PointerEventData.InputButton.Left)
                base.OnPointerUp(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("exit: " + eventData.button, this);

            // NE CHANGE RIEN ici : on ne fait rien sur exit, le bouton reste pressed tant qu’un bouton est maintenu
            base.OnPointerExit(eventData);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("click: " + eventData.button, this);
            base.OnPointerClick(eventData);
        }
    }
}