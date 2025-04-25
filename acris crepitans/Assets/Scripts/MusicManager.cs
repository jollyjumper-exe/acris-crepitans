using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] private AudioSource musicSource;
    private AudioLowPassFilter lowPassFilter;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        lowPassFilter = musicSource.GetComponent<AudioLowPassFilter>();
    }

    private void Start()
    {
        if (musicSource != null && !musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    public void MuffleMusic(bool muffle)
    {
        if (lowPassFilter == null) return;

        lowPassFilter.cutoffFrequency = muffle ? 500f : 22000f; 
    }

    private void OnDestroy()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
}
