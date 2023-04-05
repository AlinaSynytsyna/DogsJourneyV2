using System.IO;
using UnityEngine;

public static class CustomInputManager
{
    private static CustomInput Instance;
    private static readonly string _customInputPathRelease = $"{Application.streamingAssetsPath}/input.dat";
    private static readonly string _customInputPathDebug = $"{Directory.GetCurrentDirectory()}/input.dat";

    public static void SaveCustomInputKeys(CustomInput customInput)
    {
        string _customInputJson = JsonUtility.ToJson(customInput);

        if (Application.isEditor)
        {
            File.WriteAllText(_customInputPathDebug, _customInputJson);
        }
        else
        {
            File.WriteAllText(_customInputPathRelease, _customInputJson);
        }
    }

    public static CustomInput GetCustomInputKeys()
    {
        if (Instance != null)
        {
            return Instance;
        }

        Instance = new CustomInput()
        {
            Left = KeyCode.A,
            Right = KeyCode.D,
            Jump = KeyCode.Space,
            SpecialAbility = KeyCode.E,
            Interact = KeyCode.F,
            ChangeCharacter = KeyCode.Q
        };

        try
        {
            string _customInputJson = Application.isEditor ? File.ReadAllText(_customInputPathDebug) : File.ReadAllText(_customInputPathRelease);
            var _customInput = JsonUtility.FromJson<CustomInput>(_customInputJson);

            if (_customInput != null)
            {
                Instance = _customInput;
            }

            return Instance;
        }
        catch
        {
            return Instance;
        }
    }
}