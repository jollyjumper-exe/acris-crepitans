using UnityEngine;

public class AudioManager : MonoBehaviour
{
        public static AudioManager Instance;

        [Header("Music")]
        [SerializeField] private AudioSource musicSource;
        public AudioSource MusicSource => musicSource;
        private AudioLowPassFilter musicLowPassFilter;

        [Header("Sound Effects")]
        [SerializeField] private AudioSource playerSource;
        [SerializeField] private AudioClip hoverClip;
        [SerializeField] private AudioClip jumpClip;
        [SerializeField] private AudioClip coinClip;
        [SerializeField] private AudioClip coinBonusClip;
        [SerializeField] private AudioClip damageClip;
        [SerializeField] private AudioClip deadClip;


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            musicLowPassFilter = musicSource.GetComponent<AudioLowPassFilter>();
        }
        
        public void PlayJump()
        {
            DefatigueAndPlayClip(playerSource, jumpClip);
        }

        public void PlayHover()
        {
            DefatigueAndPlayClip(playerSource, hoverClip);
        }

        public void PlayCoin()
        {
            DefatigueAndPlayClip(playerSource, coinClip);
        }

        public void PlayCoinBonus()
        {
            DefatigueAndPlayClip(playerSource, coinBonusClip);
        }

        public void PlayDamage()
        {
            DefatigueAndPlayClip(playerSource, damageClip);
        }

        public void PlayDead()
        {
            DefatigueAndPlayClip(playerSource, deadClip);
        }

        private void DefatigueAndPlayClip(AudioSource source, AudioClip clip)
        {
            if (source != null && clip != null)
            {
                source.pitch = Random.Range(0.95f, 1.05f);
                source.volume = Random.Range(0.9f, 1.0f);
                source.PlayOneShot(clip);
            }
        }

        public void StartMusic()
        {
            if (musicSource != null && !musicSource.isPlaying)
            {
                musicSource.Play();
            }
        }

        public void MuffleMusic(bool muffle)
        {
            if (musicLowPassFilter == null) return;

            musicLowPassFilter.cutoffFrequency = muffle ? 500f : 22000f; 
        }

        private void OnDestroy()
        {
            if (musicSource != null && musicSource.isPlaying)
            {
                musicSource.Stop();
            }
        }
}
