using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialTriggersManager
{
    private static Dictionary<string, bool> TutorialTriggers;

    public static void LoadAllTutorialTriggers()
    {
        TutorialTriggers = null;
        if (!LevelManager.IsReloadingLevel && LevelManager.GetLevelIndex() == SceneManager.GetActiveScene().buildIndex)
        {
            TutorialTriggers = LevelManager.GetTutorialTriggers();
        }

        if (TutorialTriggers is null)
        {
            GetAllTutorialTriggers();
        }
    }

    public static void GetAllTutorialTriggers()
    {
        TutorialTriggers = new();
        var keys = GameObject.FindObjectsOfType<TutorialTrigger>();

        foreach (var key in keys)
        {
            Debug.Log(key.Id + key.IsActive);
            TutorialTriggers.Add(key.Id, key.IsActive);
        }
    }

    public static void SaveTutorialTriggers()
    {
        GetAllTutorialTriggers();
        LevelManager.SaveTutorialTriggers(TutorialTriggers);
    }

    public static bool GetIsTutorialTriggerActive(string tutorialTriggerId)
    {
        return TutorialTriggers[tutorialTriggerId];
    }
}