using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public enum DialogueType { ZimaDialogue, RedDialogue, Both }

public class DialogueRunTrigger : BaseTrigger
{
    public string YarnScriptForZima;
    public string YarnScriptForRed;
    public UnityEvent OnDialogueEndedAction;

    public DialogueType DialogueType;

    private PlayerDialogueManager _playerDialogueManager;
    private DialogueRunner _dialogueRunner;
    private bool _isShowingIcon;

    public new void Awake()
    {
        base.Awake();
        _playerDialogueManager = FindObjectOfType<PlayerDialogueManager>();
        _dialogueRunner = FindObjectOfType<DialogueRunner>();
    }

    public new void OnTriggerEnter2D(Collider2D entity)
    {
        if (entity.tag == PlayerEntityTag && IsActive)
        {
            ActivePlayer = FindActivePlayer();

            switch (DialogueType)
            {
                case DialogueType.ZimaDialogue:
                    if (ActivePlayer is Zima)
                        _isShowingIcon = true;
                    break;
                case DialogueType.RedDialogue:
                    if (ActivePlayer is Red)
                        _isShowingIcon = true;
                    break;
                case DialogueType.Both:
                    _isShowingIcon = true;
                    break;
            }

            if (ActivePlayer != null)
                Renderer.gameObject.SetActive(_isShowingIcon);
        }
    }

    public void OnTriggerStay2D(Collider2D entity)
    {
        if (entity.tag == PlayerEntityTag && IsActive)
        {
            ActivePlayer = FindActivePlayer();

            var scriptToLoad = ActivePlayer is Zima ? YarnScriptForZima : YarnScriptForRed;

            if (ActivePlayer != null && Input.GetKeyDown(CustomInput.Interact) && _isShowingIcon)
            {
                _playerDialogueManager.PlayerInDialogue = ActivePlayer;
                _playerDialogueManager.OnDialogueEnded = OnDialogueEndedAction;
                _dialogueRunner.StartDialogue(scriptToLoad);
            }
        }
    }

    public new void OnTriggerExit2D(Collider2D entity)
    {
        if (entity.tag == PlayerEntityTag && IsActive)
        {
            ActivePlayer = null;
            _isShowingIcon = false;
            Renderer.gameObject.SetActive(false);
        }
    }
}
