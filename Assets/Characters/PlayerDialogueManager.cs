using UnityEngine;

public class PlayerDialogueManager : MonoBehaviour
{
    public Player PlayerInDialogue;

    public void DialogueStarted()
    {
        PlayerInDialogue.IsTalking = true;
        PlayerInDialogue.MarkPlayerAsUnplayable();
    }

    public void DialogueEnded()
    {
        PlayerInDialogue.IsTalking = false;
        PlayerInDialogue.MarkPlayerAsPlayable();
        PlayerInDialogue = null;
    }
}
