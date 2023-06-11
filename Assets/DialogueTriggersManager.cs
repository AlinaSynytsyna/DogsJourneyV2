using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class DialogueTriggersManager
{
    private static Dictionary<string, DialogueStats> DialogueTriggers;

    public static void LoadAllDialogueTriggers()
    {
        DialogueTriggers = null;
        if (!LevelManager.IsReloadingLevel && LevelManager.GetLevelIndex() == SceneManager.GetActiveScene().buildIndex)
        {
            DialogueTriggers = LevelManager.GetDialogueTriggers();
        }

        if (DialogueTriggers is null)
        {
            GetAllDialogueTriggers();
        }
    }

    public static void GetAllDialogueTriggers()
    {
        DialogueTriggers = new();
        var keys = GameObject.FindObjectsOfType<DialogueTrigger>();

        foreach (var key in keys)
        {
            var dialogueNames = new DialogueStats
            {
                ZimaDialogue = key.YarnScriptForZima,
                RedDialogue = key.YarnScriptForRed,
                IsActive = key.IsActive,
            };

            DialogueTriggers.Add(key.Id, dialogueNames);
        }
    }

    public static void SaveDialogueTriggers()
    {
        GetAllDialogueTriggers();
        LevelManager.SaveDialogueTriggers(DialogueTriggers);
    }

    public static string GetZimaDialogue(string dialogueTriggerId)
    {
        return DialogueTriggers[dialogueTriggerId].ZimaDialogue;
    }

    public static string GetRedDialogue(string dialogueTriggerId)
    {
        return DialogueTriggers[dialogueTriggerId].RedDialogue;
    }

    public static bool GetIsDialogueTriggerActive(string dialogueTriggerId)
    {
        return DialogueTriggers[dialogueTriggerId].IsActive;
    }
}