using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AudioMute : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private Sprite mute, unmute;
    private Image image;
    private Button button;

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
    }

    private void Start()
    {
        if (AudioManager.Instance.isMuted)
        {
            image.sprite = mute;
        }
        else
        {
            image.sprite = unmute;
        }
        button.onClick.AddListener(() => AudioManager.Instance.PlaySFX(AudioManager.Instance.clickBtnClip));
        button.onClick.AddListener(MuteMusic);
        button.onClick.AddListener(AudioManager.Instance.MuteAll);
    }

    public void MuteMusic()
    {
        if (AudioManager.Instance.isMuted)
        {
            image.sprite = unmute;
        }
        else
        {
            image.sprite = mute;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.hoverBtnClip);
    }
}
