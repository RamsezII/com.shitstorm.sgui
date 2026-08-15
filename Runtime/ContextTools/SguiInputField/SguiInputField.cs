using _ARK_;
using _UTIL_;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _SGUI_
{
    public class SguiInputField : MonoBehaviour, IMGUI_global.IEscapeUser
    {
        public static SguiInputField instance;

        [SerializeField] RectTransform rt;
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] TMP_InputField inputfield;
        [SerializeField] Traductable placeholder;
        readonly ValueNotifier<bool> toggle = new();
        Action<string> onSubmit, onCancel;
        GameObject selectedObject;

        //----------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;
            rt = (RectTransform)transform.Find("rt");
            canvasGroup = GetComponent<CanvasGroup>();
            inputfield = rt.Find("inputfield").GetComponent<TMP_InputField>();
            placeholder = inputfield.placeholder.GetComponent<Traductable>();
        }

        //----------------------------------------------------------------------------------------------------------

        private void OnEnable()
        {
            UsageManager.AddUser(this, UsageGroups.Keyboard);
            IMGUI_global.instance.escape_users.AddElement(this);
        }

        private void OnDisable()
        {
            UsageManager.RemoveUser(this);
            IMGUI_global.instance.escape_users.RemoveElement(this);
        }

        //----------------------------------------------------------------------------------------------------------

        private void Start()
        {
            inputfield.onSubmit.AddListener(result => onSubmit?.Invoke(result));

            inputfield.onEndEdit.AddListener(text =>
            {
                if (inputfield.wasCanceled)
                    onCancel?.Invoke(text);
                onSubmit = onCancel = null;
                toggle.Value = false;
            });

            toggle.AddListener(value =>
            {
                canvasGroup.interactable = value;
                canvasGroup.blocksRaycasts = value;

                if (value)
                {
                    gameObject.SetActive(true);
                    canvasGroup.alpha = 0;
                }
                else
                {
                    if (EventSystem.current.currentSelectedGameObject == null)
                        EventSystem.current.SetSelectedGameObject(selectedObject);
                    selectedObject = null;
                }
            });
        }

        private void LateUpdate()
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, toggle._value ? 1 : 0, 12 * Time.unscaledDeltaTime);
            if (!toggle._value && canvasGroup.alpha == 0)
                gameObject.SetActive(false);
        }

        //----------------------------------------------------------------------------------------------------------

        void IMGUI_global.IEscapeUser.OnPressedEscape()
        {
        }

        static void CopyTextOptions(in TMP_Text source, in TMP_Text target)
        {
            target.fontSize = source.fontSize;
            target.alignment = source.alignment;
            target.font = source.font;
            target.margin = source.margin;
        }

        public void ShowHere(
            in TMP_Text tmptext,
            in Action<string> onSubmit,
            in Action<string> onCancel = null,
            in TMP_InputField.ContentType contentType = TMP_InputField.ContentType.Standard,
            in Traductions placeholder = default
        )
        {
            this.onSubmit = onSubmit;
            this.onCancel = onCancel;

            toggle.Value = true;

            rt.position = tmptext.rectTransform.position;
            rt.sizeDelta = tmptext.rectTransform.rect.size;

            CopyTextOptions(tmptext, this.placeholder.tmpro);
            this.placeholder.SetTraductions(placeholder);

            CopyTextOptions(tmptext, inputfield.textComponent);
            inputfield.contentType = contentType;
            inputfield.text = string.Empty;

            selectedObject = EventSystem.current.currentSelectedGameObject;
            EventSystem.current.SetSelectedGameObject(inputfield.gameObject);
        }
    }
}