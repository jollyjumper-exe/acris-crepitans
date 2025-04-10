using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    public Image backgroundImage;  // Reference to the background image
    public Canvas canvas;  // Reference to the canvas (if you want to resize it)
    public RectTransform canvasRectTransform;  // Reference to the canvas RectTransform

    private Camera mainCamera;

    private void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (canvas == null)
        {
            canvas = GetComponent<Canvas>();
        }

        AdjustCanvasToCamera();

        // Ensure canvas and RectTransform are set
        if (canvas == null)
            canvas = GetComponent<Canvas>();

        if (canvasRectTransform == null)
            canvasRectTransform = canvas.GetComponent<RectTransform>();
    }

    private void Update()
    {
        //so empty :()
    }

    // Method to change background color to a random saturated color
    public void ChangeBackgroundColor()
    {
        Color randomColor = GetRandomSaturatedColor();
        backgroundImage.color = randomColor;
    }

    // Generate a random saturated color
    private Color GetRandomSaturatedColor()
    {
        float hue = Random.Range(0f, 1f);
        return Color.HSVToRGB(hue, 1f, 1f);
    }

    private void AdjustCanvasToCamera()
    {
        if (canvas.renderMode != RenderMode.WorldSpace)
        {
            canvas.renderMode = RenderMode.WorldSpace;
        }

        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(cameraWidth, cameraHeight);
    }
}
