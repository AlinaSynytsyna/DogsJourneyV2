using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    public List<string> PlayableCharacters;

    public void Start()
    {
        LevelManager.GetLevelInfo();
        HealingObjectsManager.GetAllHealingObjects();
    }

    public bool CheckIfTheCharacterIsPlayable(string PlayerName)
    {
        return PlayableCharacters.Contains(PlayerName);
    }
}
