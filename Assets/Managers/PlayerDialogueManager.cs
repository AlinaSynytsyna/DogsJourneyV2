using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerDialogueManager : MonoBehaviour
{
    public Player PlayerInDialogue;
    public UnityEvent OnDialogueEnded;
    private Button _quitButton;

    public void Awake()
    {
       _quitButton = FindObjectsOfType<Button>().Where(x => x.name.Equals("Quit")).First();
    }

    public void DialogueStarted()
    {
        PlayerInDialogue.IsTalking = true;
        PlayerInDialogue.MarkPlayerAsUnplayable();
        _quitButton.interactable = false;
    }

    public void DialogueEnded()
    {
        OnDialogueEnded?.Invoke();

        PlayerInDialogue.IsTalking = false;
        PlayerInDialogue.MarkPlayerAsPlayable();
        PlayerInDialogue = null;
        OnDialogueEnded = null;
        _quitButton.interactable = true;
    }
}
