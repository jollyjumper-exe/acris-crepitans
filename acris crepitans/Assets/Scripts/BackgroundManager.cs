using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager Instance;

    [Header("UI References")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform canvasRectTransform;

    [Header("Response to Audio Settings")]
    [SerializeField] private bool useRMS = false;
    [SerializeField] private float hueScale = 10f;
    [SerializeField] private float brightness = 0.6f;
    [SerializeField] private float saturationBoost = 2f;
    [SerializeField] private int spectrumSize = 64;

    private Camera mainCamera;
    private AudioSource audioSource;
    private float[] spectrum;

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

        if (canvas == null)
            canvas = GetComponent<Canvas>();

        if (canvasRectTransform == null)
            canvasRectTransform = canvas.GetComponent<RectTransform>();

        AdjustCanvasToCamera();

        spectrum = new float[spectrumSize];

        audioSource = AudioManager.Instance.MusicSource;

        backgroundImage.color = Color.gray;
    }

    private void Update()
    {
        if (audioSource == null || !audioSource.isPlaying) return;

        float hue = Mathf.Clamp01(GetLoudness() * hueScale);
        float saturation = GetSaturationFromFrequency();

        Color targetColor = Color.HSVToRGB(hue, saturation, brightness);
        backgroundImage.color = Color.Lerp(backgroundImage.color, targetColor, Time.deltaTime * 3f);
    }

    private float GetLoudness()
    {
        if (useRMS)
        {
            float[] samples = new float[256];
            audioSource.GetOutputData(samples, 0);
            float sum = 0f;
            for (int i = 0; i < samples.Length; i++)
                sum += samples[i] * samples[i];
            return Mathf.Sqrt(sum / samples.Length);
        }
        else
        {
            audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
            float sum = 0f;
            foreach (float f in spectrum)
                sum += f;
            return sum;
        }
    }

    private float GetSaturationFromFrequency()
    {
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        float lowFreq = 0f;
        float highFreq = 0f;
        int half = spectrum.Length / 2;

        for (int i = 0; i < half; i++)
            lowFreq += spectrum[i];
        for (int i = half; i < spectrum.Length; i++)
            highFreq += spectrum[i];

        float ratio = highFreq / (lowFreq + 0.001f);
        return Mathf.Clamp01(ratio * saturationBoost);
    }

    private void AdjustCanvasToCamera()
    {
        if (canvas.renderMode != RenderMode.WorldSpace)
            canvas.renderMode = RenderMode.WorldSpace;

        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        canvasRectTransform.sizeDelta = new Vector2(cameraWidth, cameraHeight);
    }
}
