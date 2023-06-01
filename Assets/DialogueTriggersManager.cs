using System.Collections.Generic;
using UnityEngine;

public static class DialogueTriggersManager
{
    private static Dictionary<string, DialogueNames> DialogueTriggers;

    public static void LoadAllDialogueTriggers()
    {
        DialogueTriggers = LevelManager.GetDialogueTriggers();

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
            var dialogueNames = new DialogueNames
            {
                ZimaDialogue = key.YarnScriptForZima,
                RedDialogue = key.YarnScriptForRed,
            };

            DialogueTriggers.Add(key.DialogueTriggerId, dialogueNames);
        }
    }

    public static void SaveHealingObjects()
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
}