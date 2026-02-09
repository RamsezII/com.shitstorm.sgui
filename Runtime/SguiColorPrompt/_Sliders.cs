using UnityEngine;

namespace _SGUI_
{
    partial class SguiColorPrompt
    {
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