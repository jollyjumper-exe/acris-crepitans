using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private BackgroundManager backgroundManager;
    [SerializeField] private Transform playerVisual;

    [SerializeField] private float jumpDistance = 3f;
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float jumpDuration = 0.2f;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private float screenOffset = 0.5f; // World units from screen edge

    private bool isOnLeft = true;
    private bool isJumping = false;

    private Vector3 leftPos;
    private Vector3 rightPos;

    private void Start()
    {
        if (mainCamera == null)
        mainCamera = Camera.main;

        // World positions at screen left and right, with padding
        Vector3 leftWorld = mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height / 2f, mainCamera.nearClipPlane));
        Vector3 rightWorld = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2f, mainCamera.nearClipPlane));

        // Offset from screen edges
        leftWorld.x += screenOffset;
        rightWorld.x -= screenOffset;

        // Fix Y and Z to match player
        float y = playerVisual.position.y;
        float z = playerVisual.position.z;
        leftPos = new Vector3(leftWorld.x, y, z);
        rightPos = new Vector3(rightWorld.x, y, z);

        // Start on the left side
        playerVisual.position = leftPos;
        isOnLeft = true;
    }
    private void Update()
    {
        if (inputManager.isTapped && !isJumping)
        {
            Jump();
            backgroundManager.ChangeBackgroundColor();
        }
    }

    private void Jump()
    {
        isJumping = true;
        Vector3 target = isOnLeft ? rightPos : leftPos;
        isOnLeft = !isOnLeft;
        StartCoroutine(JumpTo(target));
    }

    private System.Collections.IEnumerator JumpTo(Vector3 target)
    {
        Vector3 start = playerVisual.localPosition;
        float elapsed = 0f;

        while (elapsed < jumpDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / jumpDuration;

            float yOffset = Mathf.Sin(t * Mathf.PI) * jumpHeight;
            playerVisual.localPosition = Vector3.Lerp(start, target, t) + Vector3.up * yOffset;

            yield return null;
        }

        playerVisual.localPosition = target;
        isJumping = false;
    }
}
