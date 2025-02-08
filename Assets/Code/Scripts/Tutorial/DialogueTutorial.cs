using Febucci.UI;
using UnityEngine;

public class DialogueTutorial : MonoBehaviour
{
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
