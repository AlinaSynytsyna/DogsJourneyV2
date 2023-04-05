using UnityEngine;

public class DialogueRunTrigger : BaseTrigger
{
    //private NPC _NPC;
    //private NodeVisitedTracker _nodeVisitedTracker;

    public new void Awake()
    {
        base.Awake();

        //_NPC = GetComponentInParent<NPC>();
        //if (_NPC == null)
        //    _NPC = GetComponent<NPC>();
        //_nodeVisitedTracker = FindObjectOfType<NodeVisitedTracker>();
    }

    public void OnTriggerStay2D(Collider2D entity)
    {
        //if (ActivePlayer != null && Input.GetKeyDown(CustomInput.Interact))
        //{
        //    _nodeVisitedTracker.dialogueRunner = ActivePlayer.Runner;
        //    _nodeVisitedTracker.dialogueRunner.StartDialogue(_NPC.talkToNode);
        //
    }
}
