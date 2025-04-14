using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [SerializeField] private InputManager inputManager;
    [SerializeField] private Transform avatar;

    [SerializeField] private float jumpDistance = 3f;
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float jumpDuration = 0.2f;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private float screenOffset = 0.5f;

    [SerializeField] private GameObject deathEffectPrefab;

    private bool isOnLeft = true;
    private bool isJumping = false;

    private Vector3 leftPos;
    private Vector3 rightPos;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        Vector3 leftWorld = mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height / 2f, mainCamera.nearClipPlane));
        Vector3 rightWorld = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2f, mainCamera.nearClipPlane));

        leftWorld.x += screenOffset;
        rightWorld.x -= screenOffset;

        float screenY = Screen.height * 0.25f;
        Vector3 worldY = mainCamera.ScreenToWorldPoint(new Vector3(0, screenY, mainCamera.nearClipPlane));
        float y = worldY.y;
        float z = avatar.position.z;

        leftPos = new Vector3(leftWorld.x, y, z);
        rightPos = new Vector3(rightWorld.x, y, z);

        avatar.position = leftPos;
        isOnLeft = true;
    }

    private void Update()
    {   
        if(GameManager.Instance.CurrentState != GameState.Playing) return;
        if (inputManager.isTapped && !isJumping)
        {
            Jump();
        }

        float crawlHeight = GameManager.Instance.CrawledHeight;
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
        Vector3 start = avatar.localPosition;
        float elapsed = 0f;

        while (elapsed < jumpDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / jumpDuration;

            float yOffset = Mathf.Sin(t * Mathf.PI) * jumpHeight;
            avatar.localPosition = Vector3.Lerp(start, target, t) + Vector3.up * yOffset;

            yield return null;
        }

        avatar.localPosition = target;
        isJumping = false;
    }

    public void ReportCollision(Collider other)
    {
        if(other.gameObject.CompareTag("Obstacle")) GameManager.Instance.TakeDamage(1);
        else if(other.gameObject.CompareTag("Coin")) {other.gameObject.GetComponent<Coin>().Collect(); GameManager.Instance.ReceiveCoin();}
    }

    public void PlayDamageAnimation()
    {
        StartCoroutine(FlickerCoroutine());
    }

    private IEnumerator FlickerCoroutine()
    {   
        float flickerDuration = 2f;
        float flickerInterval = 0.1f;

        Collider[] colliders = avatar.GetComponentsInChildren<Collider>();
        foreach (var col in colliders)
            col.enabled = false;

        float timer = 0f;
        bool visible = true;

        MeshRenderer[] renderers = avatar.GetComponentsInChildren<MeshRenderer>();
        if (renderers.Length == 0)
        {
            Debug.LogWarning("Avatar has no MeshRenderers.");
            yield break;
        }

        while (timer < flickerDuration)
        {
            visible = !visible;

            foreach (var rend in renderers)
            {
                rend.enabled = visible;
            }

            yield return new WaitForSeconds(flickerInterval);
            timer += flickerInterval;
        }

        foreach (var rend in renderers)
        {
            rend.enabled = true;
        }

        foreach (var col in colliders)
            col.enabled = true;
    }

    public void KillPlayer()
    {
        StopAllCoroutines();
        isJumping = false;

        if (avatar != null)
        {
            Instantiate(deathEffectPrefab, avatar.position, Quaternion.identity);
            Destroy(avatar.gameObject);
        }
    }

    public void UpdateHitPoints(int hitPoints){
        avatar.gameObject.GetComponent<Avatar>().UpdateHitPoints(hitPoints);
    }
}

