using UnityEngine;
using Yarn.Unity;

public class DialogueRunTrigger : BaseTrigger
{
    public string YarnScriptToLoad;

    private PlayerDialogueManager _playerDialogueManager;
    private DialogueRunner _dialogueRunner;

    public new void Awake()
    {
        base.Awake();
        _playerDialogueManager = FindObjectOfType<PlayerDialogueManager>();
        _dialogueRunner = FindObjectOfType<DialogueRunner>();
    }

    public void OnTriggerStay2D(Collider2D entity)
    {
        if (ActivePlayer != null && Input.GetKeyDown(CustomInput.Interact))
        {
            _playerDialogueManager.PlayerInDialogue = ActivePlayer;
            _dialogueRunner.StartDialogue(YarnScriptToLoad);
        }
    }
}
