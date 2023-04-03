using System.IO;
using UnityEngine;

public static class CustomSettingsSaveLoadManager
{
    private static CustomSettings Instance;
    private static readonly string _customSettingsPathDebug = $"{Directory.GetCurrentDirectory()}/settings.dat";
    private static readonly string _customSettingsPathRelease = $"{Application.streamingAssetsPath}/settings.dat";

    public static void SaveCustomSettings(CustomSettings customSettings)
    {
        var _customSettingsJson = JsonUtility.ToJson(customSettings);

        if (Application.isEditor)
        {
            File.WriteAllText(_customSettingsPathDebug, _customSettingsJson);
        }
        else
        {
            File.WriteAllText(_customSettingsPathRelease, _customSettingsJson);
        }
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
