using TMPro;

public static partial class Util_sgui
{
    public static float GetInvisibleHeight(this TMP_Text tmp_text)
    {
        float text_height = 0;
        string text = tmp_text.text.Trim('​');

        if (!string.IsNullOrEmpty(text))
        {
            float width = tmp_text.rectTransform.rect.width;
            text = $"L{text[1..^1]}L";
            text_height = tmp_text.GetPreferredValues(text, width, float.MaxValue).y;
        }

        return text_height;
    }
}