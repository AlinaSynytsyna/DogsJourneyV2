using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class LevelManager
{
    private static readonly string _healingObjectsPathRelease = $"{Application.streamingAssetsPath}/LevelInfo.dat";

    private static readonly string _healingObjectsPathDebug = $"{Directory.GetCurrentDirectory()}/LevelInfo.dat";

    private static JObject _levelInfoJObject;

    public static void GetLevelInfo()
    {
        var path = Application.isEditor ? _healingObjectsPathDebug : _healingObjectsPathRelease;

        try
        {
            var levelInfoJson = File.ReadAllText(path);
            _levelInfoJObject = JObject.Parse(levelInfoJson);
        }
        catch
        {
            _levelInfoJObject = new JObject();
        }
    }

    public static Dictionary<string, bool> GetHealingObjects()
    { 
        try
        {
            var healingObjectsJObject = _levelInfoJObject[Constants.HealingObjects];

            return healingObjectsJObject.ToObject<Dictionary<string, bool>>();
        }
        catch
        {
            return null;
        }
    }

    public static void SaveHealingObjects(Dictionary<string, bool> healingObjectsDictionary)
    {
        var healingObjectsJObject = JObject.Parse(JsonConvert.SerializeObject(healingObjectsDictionary, Formatting.Indented));

        try
        {
            _levelInfoJObject[Constants.HealingObjects] = healingObjectsJObject;
        }
        catch
        {
            _levelInfoJObject.Add(healingObjectsJObject);
        }
    }

    public static void SaveLevelInfo()
    {
        var path = Application.isEditor ? _healingObjectsPathDebug : _healingObjectsPathRelease;

        File.WriteAllText(path, JsonConvert.SerializeObject(_levelInfoJObject, Formatting.Indented));
    }
}