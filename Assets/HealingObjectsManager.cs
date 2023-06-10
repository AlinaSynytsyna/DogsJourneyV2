using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class HealingObjectsManager
{
    private static Dictionary<string, bool> HealingObjects;

    public static void GetAllHealingObjects()
    {
        if (!LevelManager.IsReloadingLevel)
        {
            HealingObjects = LevelManager.GetHealingObjects();
        }

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

    public static void MarkHealingObjectAsActive(string healingObjectId)
    {
        HealingObjects[healingObjectId] = true;
    }

    public static void MarkAllHealingObjectsAsActive()
    {
        foreach (var key in HealingObjects.Keys.ToList())
            MarkHealingObjectAsActive(key);
    }

    public static void SaveHealingObjects()
    {
        LevelManager.SaveHealingObjects(HealingObjects);
    }

    public static bool IsHealingObjectActive(string healingObjectId)
    {
        return HealingObjects[healingObjectId];
    }
}
