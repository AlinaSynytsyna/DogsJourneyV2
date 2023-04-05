using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    public List<string> PlayableCharacters;

    public bool CheckCharacter(string PlayerName)
    {
        if (PlayableCharacters.Contains(PlayerName))
            return true;
        else return false;
    }
}
