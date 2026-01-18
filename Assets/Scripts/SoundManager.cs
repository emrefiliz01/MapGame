using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music")]
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioClip[] battleMusic;
    [SerializeField] private AudioClip winMusic;
    [SerializeField] private AudioClip loseMusic;

    [Header("SFX")]
    [SerializeField] private AudioClip clickSound;

    [Header("Volume")]
    [Range(0f, 1f)]
    [SerializeField] private float musicVolume = 0.5f;
    [Range(0f, 1f)]
    [SerializeField] private float sfxVolume = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null) musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (sfxSource != null) sfxSource.volume = sfxVolume;
    }

    public void PlayBackgroundMusic()
    {
        if (musicSource == null || backgroundMusic == null) return;
        
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void PlayBattleMusic()
    {
        if (musicSource == null || battleMusic == null || battleMusic.Length == 0) return;
        
        int randomIndex = Random.Range(0, battleMusic.Length);
        musicSource.clip = battleMusic[randomIndex];
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void PlayWinMusic()
    {
        if (musicSource == null || winMusic == null) return;
        
        musicSource.clip = winMusic;
        musicSource.loop = false;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void PlayLoseMusic()
    {
        if (musicSource == null || loseMusic == null) return;
        
        musicSource.clip = loseMusic;
        musicSource.loop = false;
        musicSource.volume = musicVolume;
        musicSource.Play();
    }

    public void PlayClickSound()
    {
        if (sfxSource == null || clickSound == null) return;
        
        sfxSource.PlayOneShot(clickSound, sfxVolume);
    }
}
