using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CharacterSwitcher", menuName = "Character Switcher", order = 53)]
public class CharacterChanger : ScriptableObject
{
    public List<Player> ArrayPlayers = new List<Player>();
    public int ActiveCharacter;
    public void Awake()
    {
        FindCharacters();
    }
    public void FindCharacters()
    {
        ArrayPlayers.Clear();
        ArrayPlayers.Add(FindObjectOfType<Zima>());
        ArrayPlayers.Add(FindObjectOfType<Red>());
    }
    public void SwitchCharacter()
    {
        if (ArrayPlayers.Count > 1)
        {
            if (ActiveCharacter == ArrayPlayers.Count - 1)
                ActiveCharacter = 0;
            else
                ActiveCharacter++;
        }
    }
}
