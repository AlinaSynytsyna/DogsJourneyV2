using System.Collections.Generic;
using UnityEngine;

public static class HealingObjectsManager
{
    private static Dictionary<string, bool> HealingObjects;

    public static void GetAllHealingObjects()
    {
        HealingObjects = LevelManager.GetHealingObjects();

        if (HealingObjects is null)
        {
            HealingObjects = new();
            var keys = GameObject.FindObjectsOfType<HealingObject>();

            foreach (var key in keys)
            {
                HealingObjects.Add(key.HealingObjectId, true);
            }
        }
    }

    public static void MarkHealingObjectAsDestroyed(string healingObjectId)
    {
        HealingObjects[healingObjectId] = false;
    }

    public static void SaveHealingObjects()
    {
        LevelManager.SaveHealingObjects(HealingObjects);
    }

    public static bool IsHealingObjectDestroyed(string healingObjectId)
    {
        return HealingObjects[healingObjectId];
    }
}
