using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class AudioBtn : MonoBehaviour, IPointerEnterHandler
{
    private Button button;


    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(() => AudioManager.Instance.PlaySFX(AudioManager.Instance.clickBtnClip));
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.hoverBtnClip);
    }
}
