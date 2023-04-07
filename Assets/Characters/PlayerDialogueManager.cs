using UnityEngine;

public class PlayerDialogueManager : MonoBehaviour
{
    public Player PlayerInDialogue;

    public void DialogueStarted()
    {
        PlayerInDialogue.PlayerStartedDialogue();
    }

    public void DialogueEnded()
    {
        PlayerInDialogue.PlayerEndedDialogue();
        PlayerInDialogue = null;
    }
}
