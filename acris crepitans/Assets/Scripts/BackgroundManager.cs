using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    public Image backgroundImage;
    public void ChangeBackgroundColor()
    {
        Color randomColor = GetRandomSaturatedColor();
        backgroundImage.color = randomColor;
    }

    private Color GetRandomSaturatedColor()
    {
        float hue = Random.Range(0f, 1f);
        return Color.HSVToRGB(hue, 1f, 1f);
    }
}
