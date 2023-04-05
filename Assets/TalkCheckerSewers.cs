using UnityEngine;

public class TalkCheckerSewers : MonoBehaviour
{
    //public NodeVisitedTracker Tracker;
    public TutorialTrigger RedTutorial;
    //public DialogueRunTrigger Interact;
    public TutorialTrigger ChangeTutorial;
    private bool IsTalked;
    public void Awake()
    {
        RedTutorial.gameObject.SetActive(false);
        ChangeTutorial.gameObject.SetActive(false);
        IsTalked = false;
    }
    public void Update()
    {
        //if (!IsTalked && Tracker._visitedNodes.Contains("Red.HelpMe"))
        //{
        //    Interact.gameObject.SetActive(false);
        //    RedTutorial.gameObject.SetActive(true);
        //    ChangeTutorial.gameObject.SetActive(true);
        //    IsTalked = true;
        //}
    }
}
