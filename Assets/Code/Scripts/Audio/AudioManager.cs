using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Managers")]
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    [Header("Music Audio Clips")]
    [SerializeField] private AudioClip musicClip01;
    [SerializeField] private AudioClip musicClip02;
    [SerializeField] private AudioClip musicClip03;
    [SerializeField] private AudioClip musicClip04;
    [SerializeField] private AudioClip musicClip05;
    [Header("Sfx Audio Clips")]
    [SerializeField] private AudioClip gameOverClip;
    [SerializeField] private AudioClip swordClip;
    [SerializeField] private AudioClip enemyShootClip;
    [SerializeField] private AudioClip shootClip;
    [SerializeField] private AudioClip throwGranadeClip;
    [SerializeField] private AudioClip clickBtnClip;
    [SerializeField] private AudioClip interactContainerClip;
    [SerializeField] private AudioClip explosionClip;
    [SerializeField] private AudioClip killEnemyClip;
    [SerializeField] private AudioClip hoverBtnClip;
    [SerializeField] private AudioClip IntroGameBtnClip;
}
