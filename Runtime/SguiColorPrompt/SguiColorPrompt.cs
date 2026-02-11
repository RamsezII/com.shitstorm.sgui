using _SGUI_.prompts.color_prompt;
using _UTIL_;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _SGUI_
{
    public sealed partial class SguiColorPrompt : MonoBehaviour
    {
        public enum Modes : byte
        {
            RGB_0_255,
            RGB_0_1,
            HSV_0_1,
        }

        public static SguiColorPrompt instance;

        [SerializeField] Color color;
        [SerializeField] RectTransform rt;
        [SerializeField] ColorPrompt_color current_color, new_color;
        [SerializeField] RectTransform rt_disk, rt_disk_sel, rt_square, rt_square_sel;
        [SerializeField] Graphic squareGradient_h, squareGradient_v;
        [SerializeField] TMP_Dropdown dropdown_mode;
        [SerializeField] ColorPromptGradient[] gradients;
        [SerializeField] TMP_InputField tmp_hex;

        public Action<Color> _onSubmit;
        public Action _onCancel, _onClose;

        [SerializeField] Modes mode;

        [SerializeField] IA_ColorPrompt IA_inputs;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            instance = this;

            rt = (RectTransform)transform.Find("rt");

            current_color = transform.Find("rt/values/current").GetComponent<ColorPrompt_color>();
            new_color = transform.Find("rt/values/color").GetComponent<ColorPrompt_color>();

            rt_disk = (RectTransform)transform.Find("rt/graphic/disk");
            rt_disk_sel = (RectTransform)transform.Find("rt/graphic/disk/selector2");
            rt_square = (RectTransform)transform.Find("rt/graphic/square");
            rt_square_sel = (RectTransform)rt_square.Find("selector1");

            squareGradient_h = transform.Find("rt/graphic/square/gradient_h").GetComponent<Graphic>();
            squareGradient_v = transform.Find("rt/graphic/square/gradient_v").GetComponent<Graphic>();
            dropdown_mode = transform.Find("rt/vlayout/mode/dropdown").GetComponent<TMP_Dropdown>();
            gradients = transform.Find("rt/vlayout").GetComponentsInChildren<ColorPromptGradient>(true);
            tmp_hex = transform.Find("rt/vlayout/hex-value/inputfield").GetComponent<TMP_InputField>();

            IA_inputs?.Dispose();
            IA_inputs = new();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnEnable()
        {
            IA_inputs.Enable();
        }

        private void OnDisable()
        {
            IA_inputs.Disable();
        }

        //--------------------------------------------------------------------------------------------------------------

        private void Start()
        {
            transform.Find("background").GetComponent<PointerClickHandler>().onClick += eventData => Cancel();

            transform.Find("rt/header/button-close").GetComponent<Button>().onClick.AddListener(Cancel);

            IA_inputs.Controls.Submit.started += _ => Submit();

            gradients[0].renderer.values.Clear();
            gradients[1].renderer.values.Clear();
            gradients[2].renderer.values.Clear();

            gradients[1].renderer.values.Add(new(0, Color.black));
            gradients[1].renderer.values.Add(new(1, Color.white));

            gradients[2].renderer.values.Add(new(0, Color.black));
            gradients[2].renderer.values.Add(new(1, Color.white));

            for (int i = 0; i < gradients.Length; ++i)
            {
                var gradient = gradients[i];
                gradient._slider.onValueChanged.AddListener(value => SetNewColor(ReadFromSliders()));
            }

            dropdown_mode.value = 2;
            dropdown_mode.RefreshShownValue();
            dropdown_mode.onValueChanged.AddListener(OnMode);
            OnMode(dropdown_mode.value);

            StartDisk();
            StartSquare();

            gameObject.SetActive(false);

#if UNITY_EDITOR
            SguiGlobal.instance._FOCUSED_RECTT = (RectTransform)gradients[0].renderer.transform;
#endif
        }

        //--------------------------------------------------------------------------------------------------------------

        void Submit()
        {
            _onSubmit?.Invoke(color);
            Close();
        }

        public void Cancel()
        {
            _onCancel?.Invoke();
            Close();
        }

        void Close()
        {
            gameObject.SetActive(false);
            _onClose?.Invoke();
            _onSubmit = null;
            _onCancel = null;
            _onClose = null;
        }

        void OnMode(int value) => OnMode((Modes)value);
        void OnMode(Modes mode)
        {
            this.mode = mode;

            gradients[0].renderer.values.Clear();

            if (mode == Modes.HSV_0_1)
            {
                gradients[0].label.text = "H";
                gradients[1].label.text = "S";
                gradients[2].label.text = "V";

                const int count = 15;
                for (int i = 0; i < count; i++)
                {
                    float t = (float)i / (count - 1);
                    gradients[0].renderer.values.Add(new(t, Color.HSVToRGB(t, 1, 1)));
                }
                gradients[0].renderer.SetVerticesDirty();
            }
            else
            {
                gradients[0].label.text = "R";
                gradients[1].label.text = "G";
                gradients[2].label.text = "B";

                gradients[0].renderer.values.Add(new(0, Color.black));
                gradients[0].renderer.values.Add(new(1, Color.white));
            }

            for (int i = 0; i < 3; i++)
            {
                var gradient = gradients[i];
                if (mode == Modes.RGB_0_255)
                {
                    gradient._slider.maxValue = 255;
                    gradient._slider.wholeNumbers = true;
                }
                else
                {
                    gradient._slider.wholeNumbers = false;
                    gradient._slider.maxValue = 1;
                }
            }

            SetNewColor(color);
        }

        public void ShowColorPrompt(in Vector2 position, in Color currentColor, in Action<Color> onSubmit, in Action onCancel = null)
        {
            gameObject.SetActive(true);
            rt.position = position;

            current_color.SetColor(currentColor);
            SetNewColor(color);

            _onSubmit = onSubmit;
            _onCancel = onCancel;
        }

        public void SetNewColor(in Color color)
        {
            this.color = color;
            new_color.SetColor(color);

            Color.RGBToHSV(color, out float H, out float S, out float V);
            Vector3 hsv = new(H, S, V);
            float angle = H * 2 * Mathf.PI;

            tmp_hex.text = ColorUtility.ToHtmlStringRGB(color);

            rt_disk_sel.localRotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
            rt_square_sel.localPosition = new Vector2(S - .5f, V - .5f) * rt_square.sizeDelta;

            gradients[3]._slider.SetValueWithoutNotify(color.a);
            squareGradient_h.color = Color.HSVToRGB(H, 1, 1);

            gradients[3]._slider.SetValueWithoutNotify(color.a);

            switch (mode)
            {
                case Modes.RGB_0_255:
                    gradients[0]._slider.SetValueWithoutNotify(color.r * 255);
                    gradients[1]._slider.SetValueWithoutNotify(color.g * 255);
                    gradients[2]._slider.SetValueWithoutNotify(color.b * 255);
                    break;

                case Modes.RGB_0_1:
                    gradients[0]._slider.SetValueWithoutNotify(color.r);
                    gradients[1]._slider.SetValueWithoutNotify(color.g);
                    gradients[2]._slider.SetValueWithoutNotify(color.b);
                    break;

                case Modes.HSV_0_1:
                    gradients[0]._slider.SetValueWithoutNotify(H);
                    gradients[1]._slider.SetValueWithoutNotify(S);
                    gradients[2]._slider.SetValueWithoutNotify(V);
                    break;
            }

            switch (mode)
            {
                case Modes.RGB_0_255:
                case Modes.RGB_0_1:
                    for (int i = 0; i < 3; i++)
                    {
                        var gradient = gradients[i];

                        for (int j = 0; j < 2; ++j)
                        {
                            Color c_rgb = color;
                            c_rgb[i] = j;
                            c_rgb.a = 1;
                            gradient.renderer.values[j] = new(j, c_rgb);
                        }

                        gradient.renderer.SetVerticesDirty();
                    }
                    break;

                case Modes.HSV_0_1:
                    for (int i = 1; i < 3; i++)
                    {
                        var gradient = gradients[i];
                        for (int j = 0; j < 2; ++j)
                        {
                            Vector3 c_hsv = hsv;
                            c_hsv[i] = j;
                            Color c_rgb = Color.HSVToRGB(c_hsv.x, c_hsv.y, c_hsv.z);
                            gradient.renderer.values[j] = new(j, c_rgb);
                        }

                        gradient.renderer.SetVerticesDirty();
                    }
                    break;
            }

            for (int i = 0; i < gradients.Length; i++)
            {
                var gradient = gradients[i];
                gradient.inputField.text = Util.FloatToString(gradient._slider.value);
            }
        }

        //--------------------------------------------------------------------------------------------------------------

        private void OnDestroy()
        {
            IA_inputs.Dispose();
        }
    }
}