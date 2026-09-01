using UnityEngine;

namespace _SGUI_
{
    partial class SguiColorPrompt
    {
        void SetNewColorFromSliders()
        {
            Color color = ReadFromSliders();

            if (mode == Modes.HSV_0_1)
            {
                Vector3 hsv = new(
                    gradients[0]._slider.value,
                    gradients[1]._slider.value,
                    gradients[2]._slider.value
                );
                SetNewColor(color, hsv);
                return;
            }

            SetNewColor(color);
        }

        void SetNewColorFromHSV(float hue, float saturation, float value)
        {
            Color newColor = Color.HSVToRGB(hue, saturation, value);
            newColor.a = color.a;
            SetNewColor(newColor, new Vector3(hue, saturation, value));
        }

        Color ReadFromSliders()
        {
            Color color = Color.clear;

            switch (mode)
            {
                case Modes.RGB_0_1:
                    color = new(
                        gradients[0]._slider.value,
                        gradients[1]._slider.value,
                        gradients[2]._slider.value,
                        gradients[3]._slider.value
                    );
                    break;

                case Modes.RGB_0_255:
                    color = new(
                        gradients[0]._slider.value / 255f,
                        gradients[1]._slider.value / 255f,
                        gradients[2]._slider.value / 255f,
                        gradients[3]._slider.value
                    );
                    break;

                case Modes.HSV_0_1:
                    color = Color.HSVToRGB(
                        gradients[0]._slider.value,
                        gradients[1]._slider.value,
                        gradients[2]._slider.value
                    );
                    color.a = gradients[3]._slider.value;
                    break;
            }

            return color;
        }
    }
}
