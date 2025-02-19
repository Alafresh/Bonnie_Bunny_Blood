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
    private string[] urls;
    [SerializeField] string[] videoNames;

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
#if UNITY_WEBGL
        SetUpVideoCLips();
#endif
    }

    private void SetUpVideoCLips()
    {
        videoPlayer.source = VideoSource.Url;
        urls = new string[videoClips.Length];
        for (int i = 0; i < videoClips.Length; i++)
        {
            urls[i] = System.IO.Path.Combine(Application.streamingAssetsPath, videoNames[i]);
        }
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
                videoRender.SetActive(true);
#if UNITY_WEBGL
                videoPlayer.url = urls[0];
#else
                videoPlayer.clip = videoClips[0];
#endif
                videoPlayer.Play();
                break;
            case 8:
#if UNITY_WEBGL
                videoPlayer.url = urls[1];
#else
                videoPlayer.clip = videoClips[1];
#endif
                videoPlayer.Play();
                break;
            case 9:
#if UNITY_WEBGL
                videoPlayer.url = urls[2];
#else
                videoPlayer.clip = videoClips[2];
#endif
                videoPlayer.Play();
                break;
            case 10:
#if UNITY_WEBGL
                videoPlayer.url = urls[3];
#else
                videoPlayer.clip = videoClips[3];
#endif
                videoPlayer.Play();
                break;
            case 11:
#if UNITY_WEBGL
                videoPlayer.url = urls[4];
#else
                videoPlayer.clip = videoClips[4];
#endif
                videoPlayer.Play();
                break;
            case 12:
#if UNITY_WEBGL
                videoPlayer.url = urls[5];
#else
                videoPlayer.clip = videoClips[5];
#endif
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
