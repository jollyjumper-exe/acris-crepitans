using UnityEngine;

public class CanvasScaler : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Canvas canvas;

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
