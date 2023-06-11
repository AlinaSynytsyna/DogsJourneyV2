﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelManager
{
    public static JObject LevelInfoJObject;

    public static bool IsReloadingLevel = false;

    public static bool HasInformation => LevelInfoJObject != null && (LevelInfoJObject[Constants.LevelInfo] != null || LevelInfoJObject[Constants.PlayerStats] != null || LevelInfoJObject[Constants.HealingObjects] != null);

    private static readonly string _healingObjectsPathRelease = $"{Application.streamingAssetsPath}/LevelInfo.dat";

    private static readonly string _healingObjectsPathDebug = $"{Directory.GetCurrentDirectory()}/LevelInfo.dat";

    public static void DeleteSaveFile()
    {
        var path = Application.isEditor ? _healingObjectsPathDebug : _healingObjectsPathRelease;

        try
        {
            File.Delete(path);
        }
        catch
        {
            return;
        }
    }

    public static void GetLevelInfo()
    {
        var path = Application.isEditor ? _healingObjectsPathDebug : _healingObjectsPathRelease;

        try
        {
            var levelInfoJson = File.ReadAllText(path);
            LevelInfoJObject = JObject.Parse(levelInfoJson);
        }
        catch
        {
            LevelInfoJObject = new JObject();
        }
    }

    public static int GetLevelIndex()
    {
        try
        {
            var levelIndexDictionaryJObject = LevelInfoJObject[Constants.LevelInfo];

            var levelIndexDictionary = levelIndexDictionaryJObject.ToObject<Dictionary<string, int>>();

            return levelIndexDictionary[Constants.LevelIndex];
        }
        catch
        {
            return 1;
        }
    }

    public static Dictionary<string, bool> GetHealingObjects()
    {
        try
        {
            var healingObjectsJObject = LevelInfoJObject[Constants.HealingObjects];

            return healingObjectsJObject.ToObject<Dictionary<string, bool>>();
        }
        catch
        {
            return null;
        }
    }

    public static Dictionary<string, bool> GetTeleportTriggers()
    {
        try
        {
            var teleportTriggersJObject = LevelInfoJObject[Constants.TeleportTriggers];

            return teleportTriggersJObject.ToObject<Dictionary<string, bool>>();
        }
        catch
        {
            return null;
        }
    }

    public static Dictionary<string, bool> GetTutorialTriggers()
    {
        try
        {
            var tutorialTriggersJObject = LevelInfoJObject[Constants.TutorialTriggers];

            return tutorialTriggersJObject.ToObject<Dictionary<string, bool>>();
        }
        catch
        {
            return null;
        }
    }

    public static Dictionary<string, DialogueStats> GetDialogueTriggers()
    {
        try
        {
            var dialogueTriggersJObject = LevelInfoJObject[Constants.DialogueTriggers];

            return dialogueTriggersJObject.ToObject<Dictionary<string, DialogueStats>>();
        }
        catch
        {
            return null;
        }
    }

    public static Dictionary<string, PlayerStats> GetPlayerStats()
    {
        try
        {
            var playerStatsJObject = LevelInfoJObject[Constants.PlayerStats];

            return playerStatsJObject.ToObject<Dictionary<string, PlayerStats>>();
        }
        catch
        {
            return null;
        }
    }

    public static void SaveDialogueTriggers(Dictionary<string, DialogueStats> dialogueTriggersDictionary)
    {
        var dialogueTriggersJObject = JObject.Parse(JsonConvert.SerializeObject(dialogueTriggersDictionary, Formatting.Indented));

        try
        {
            LevelInfoJObject[Constants.DialogueTriggers] = dialogueTriggersJObject;
        }
        catch
        {
            LevelInfoJObject.Add(dialogueTriggersJObject);
        }
    }

    public static void SaveHealingObjects(Dictionary<string, bool> healingObjectsDictionary)
    {
        var healingObjectsJObject = JObject.Parse(JsonConvert.SerializeObject(healingObjectsDictionary, Formatting.Indented));

        try
        {
            LevelInfoJObject[Constants.HealingObjects] = healingObjectsJObject;
        }
        catch
        {
            LevelInfoJObject.Add(healingObjectsJObject);
        }
    }

    public static void SaveTeleportTriggers(Dictionary<string, bool> teleportTriggersDictionary)
    {
        var teleportTriggersJObject = JObject.Parse(JsonConvert.SerializeObject(teleportTriggersDictionary, Formatting.Indented));

        try
        {
            LevelInfoJObject[Constants.TeleportTriggers] = teleportTriggersJObject;
        }
        catch
        {
            LevelInfoJObject.Add(teleportTriggersJObject);
        }
    }

    public static void SaveTutorialTriggers(Dictionary<string, bool> tutorialTriggersDictionary)
    {
        var tutorialTriggersJObject = JObject.Parse(JsonConvert.SerializeObject(tutorialTriggersDictionary, Formatting.Indented));

        try
        {
            LevelInfoJObject[Constants.TutorialTriggers] = tutorialTriggersJObject;
        }
        catch
        {
            LevelInfoJObject.Add(tutorialTriggersJObject);
        }
    }

    public static void SavePlayers()
    {
        var players = GameObject.FindObjectsOfType<Player>();
        var PlayersStatsDictionary = new Dictionary<string, PlayerStats>();

        foreach (Player player in players)
        {
            var stats = new PlayerStats
            {
                IsActive = player.IsPlayerActive,
                Health = player.Health,
                PositionX = player.gameObject.transform.position.x,
                PositionY = player.gameObject.transform.position.y,
                IsPlayableInScene = player.IsPlayableInScene

            };
            PlayersStatsDictionary.Add(player.PlayerName, stats);
        }

        var playerStatsJObject = JObject.Parse(JsonConvert.SerializeObject(PlayersStatsDictionary, Formatting.Indented));

        try
        {
            LevelInfoJObject[Constants.PlayerStats] = playerStatsJObject;
        }
        catch
        {
            LevelInfoJObject.Add(playerStatsJObject);
        }
    }

    public static void SaveLevelIndex()
    {
        var levelInfoDictionary = new Dictionary<string, int>
        {
            { "LevelIndex", SceneManager.GetActiveScene().buildIndex }
        };
        var levelIndexJObject = JObject.Parse(JsonConvert.SerializeObject(levelInfoDictionary, Formatting.Indented));

        try
        {
            LevelInfoJObject[Constants.LevelInfo] = levelIndexJObject;
        }
        catch
        {
            LevelInfoJObject.Add(levelIndexJObject);
        }
    }

    public static void SaveLevelInfo()
    {
        var path = Application.isEditor ? _healingObjectsPathDebug : _healingObjectsPathRelease;

        File.WriteAllText(path, JsonConvert.SerializeObject(LevelInfoJObject, Formatting.Indented));
    }
}