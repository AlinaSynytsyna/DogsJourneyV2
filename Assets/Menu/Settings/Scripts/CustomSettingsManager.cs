using System.IO;
using UnityEngine;

public static class CustomSettingsManager
{
    private static CustomSettings Instance;
    private static readonly string _customSettingsPathDebug = $"{Directory.GetCurrentDirectory()}/settings.dat";
    private static readonly string _customSettingsPathRelease = $"{Application.streamingAssetsPath}/settings.dat";

    public static void SaveCustomSettings(CustomSettings customSettings)
    {
        var path = Application.isEditor ? _customSettingsPathDebug : _customSettingsPathRelease;
        var _customSettingsJson = JsonUtility.ToJson(customSettings);

        File.WriteAllText(path, _customSettingsJson);
    }

    public static CustomSettings GetCustomSettings()
    {
        if (Instance != null)
        {
            return Instance;
        }

        Instance = new CustomSettings()
        {
            ScreenWidthValue = Screen.currentResolution.width,
            ScreenHeightValue = Screen.currentResolution.height,
            IsFullscreen = true,
            RefreshRate = Screen.currentResolution.refreshRate,
            MusicVolumeValue = -25,
            EffectsVolumeValue = -25
        };

        try
        {
            var _customSettingsJson = Application.isEditor ? File.ReadAllText(_customSettingsPathDebug) : File.ReadAllText(_customSettingsPathRelease);
            var _customSettings = JsonUtility.FromJson<CustomSettings>(_customSettingsJson);

            if (_customSettings != null)
            {
                Instance = _customSettings;
            }

            return Instance;
        }
        catch
        {
            return Instance;
        }
    }
}
