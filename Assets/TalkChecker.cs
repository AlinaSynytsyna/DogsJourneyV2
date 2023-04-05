using UnityEngine;

public class TalkChecker : MonoBehaviour
{
    //public NodeVisitedTracker Tracker;
    public TeleportationTrigger Teleport;
    //public DialogueRunTrigger Interact;
    public void Awake()
    {
        Teleport.gameObject.SetActive(false);
        //Interact.gameObject.SetActive(true);
    }
    public void Update()
    {
        //if (Tracker._visitedNodes.Contains("Red.BePolite"))
        //{
        //    Teleport.gameObject.SetActive(true);
        //    Interact.gameObject.SetActive(false);
        //}
    }
}
