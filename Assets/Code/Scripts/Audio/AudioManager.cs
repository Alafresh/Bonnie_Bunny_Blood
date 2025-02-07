using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Managers")]
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioSource introSource;

    [Header("Music Audio Clips")]
    [SerializeField] private AudioClip[] musicClip;
    
    [Header("Sfx Audio Clips")]
    [SerializeField] public AudioClip gameOverClip;
    [SerializeField] public AudioClip swordClip;
    [SerializeField] public AudioClip shootClip;
    [SerializeField] public AudioClip throwGranadeClip;
    [SerializeField] public AudioClip clickBtnClip;
    [SerializeField] public AudioClip interactContainerClip;
    [SerializeField] public AudioClip explosionClip;
    [SerializeField] public AudioClip killClip;
    [SerializeField] public AudioClip hoverBtnClip;

    private int currentClipIndex = 0;
    public bool isMuted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if(musicClip.Length > 0)
        {
            StartCoroutine(PlayMusic());
        }
    }

    public void MuteAll()
    {
        musicAudioSource.mute = !musicAudioSource.mute;
        sfxAudioSource.mute = !sfxAudioSource.mute;
        introSource.mute = !introSource.mute;
        isMuted = !isMuted;
    }
    public void PlaySFX(AudioClip clip)
    {
        sfxAudioSource.clip = clip;
        sfxAudioSource.PlayOneShot(clip);
    }

    IEnumerator PlayMusic()
    {

        introSource.Play();

        while (true)
        {
            musicAudioSource.clip = musicClip[currentClipIndex];
            musicAudioSource.Play();
            
            yield return new WaitForSeconds(musicAudioSource.clip.length);
            
            currentClipIndex++;
            
            if (currentClipIndex >= musicClip.Length)
            {
                currentClipIndex = 0;
            }
        }
    }
}
