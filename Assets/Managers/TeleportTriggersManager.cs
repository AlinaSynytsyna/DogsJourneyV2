using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class TeleportTriggersManager
{
    private static Dictionary<string, bool> TeleportTriggers;

    public static void LoadAllTeleportTriggers()
    {
        TeleportTriggers = null;
        if (!LevelManager.IsReloadingLevel && LevelManager.GetLevelIndex() == SceneManager.GetActiveScene().buildIndex)
        {
            TeleportTriggers = LevelManager.GetTeleportTriggers();
        }

        if (TeleportTriggers is null)
        {
            GetAllTeleportTriggers();
        }
    }

    public static void GetAllTeleportTriggers()
    {
        TeleportTriggers = new();
        var keys = GameObject.FindObjectsOfType<TeleportationTrigger>();

        foreach (var key in keys)
        {
            TeleportTriggers.Add(key.Id, key.IsActive);
        }
    }

    public static void SaveTeleportTriggers()
    {
        GetAllTeleportTriggers();
        LevelManager.SaveTeleportTriggers(TeleportTriggers);
    }

    public static bool GetIsTeleportTriggerActive(string teleportTriggerId)
    {
        return TeleportTriggers[teleportTriggerId];
    }
}