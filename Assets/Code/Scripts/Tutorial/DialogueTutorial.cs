using Febucci.UI;
using UnityEngine;
using UnityEngine.Video;

public class DialogueTutorial : MonoBehaviour
{
    [SerializeField] private GameObject videoRender;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] VideoClip[] videoClips;
    [SerializeField] GameObject dialoguePanel;
    public TypewriterByCharacter textAnimatorPlayer;

    [TextArea(3, 50), SerializeField]
    string[] textToShow;
    private int index;

    private void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(textAnimatorPlayer, $"Text Animator Player component is null in {gameObject.name}");
    }

    private void Start()
    {
        ShowText();
    }

    public void ShowText()
    {
        textAnimatorPlayer.ShowText(textToShow[0]);
    }

    public void NextText()
    {
        index++;
        switch (index)
        {
            case 7:
                
                videoPlayer.clip = videoClips[0];
                videoPlayer.Play();
                break;
            case 8:
                videoPlayer.clip = videoClips[1];
                videoPlayer.Play();
                break;
            case 9:
                videoPlayer.clip = videoClips[2];
                videoPlayer.Play();
                break;
            case 10:
                videoPlayer.clip = videoClips[3];
                videoPlayer.Play();
                break;
            case 11:
                videoPlayer.clip = videoClips[4];
                videoPlayer.Play();
                break;
            case 12:
                videoPlayer.clip = videoClips[5];
                videoPlayer.Play();
                break;
            case 13:
                videoRender.SetActive(false);
                videoPlayer.Stop();
                break;
        }
        if (index < textToShow.Length)
        {
            textAnimatorPlayer.ShowText(textToShow[index]);
        }
        else
        {
            dialoguePanel.SetActive(false);
        }
    }
}
