using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    public List<string> PlayableCharacters;
    public string MainPlayableCharacter;

    public void Awake()
    {
        LevelManager.GetLevelInfo();

        if (!LevelManager.IsReloadingLevel)
        {
            HealingObjectsManager.GetAllHealingObjects();
        }
    }

    public bool CheckIfTheCharacterIsPlayable(string playerName)
    {
        return PlayableCharacters.Contains(playerName);
    }
}
