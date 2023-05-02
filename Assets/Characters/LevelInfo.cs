using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    private CustomInput _customInput;
    private PlayerCamera _playerCamera;
    public List<Player> PlayableCharacters;
    public Player ActivePlayer;
    public string MainPlayableCharacter;

    public void Awake()
    {
        _customInput = CustomInputManager.GetCustomInputKeys();
        _playerCamera = FindObjectOfType<PlayerCamera>();
        LevelManager.GetLevelInfo();

        if (!LevelManager.IsReloadingLevel)
        {
            HealingObjectsManager.GetAllHealingObjects();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(_customInput.ChangeCharacter) && PlayableCharacters.Count > 1 && !ActivePlayer.IsTalking)
        {
            Invoke(nameof(SwitchCharacter), 0.2f);
        }
    }

    public bool CheckIfTheCharacterIsPlayable(Player playerName)
    {
        return PlayableCharacters.Contains(playerName);
    }

    public void SwitchCharacter()
    {
        _playerCamera.FadeCameraInShort();
        ActivePlayer.MarkPlayerAsUnplayable();
        var activePlayerIndex = PlayableCharacters.IndexOf(ActivePlayer) + 1 != PlayableCharacters.Count ? PlayableCharacters.IndexOf(ActivePlayer) + 1 : 0;
        ActivePlayer = PlayableCharacters[activePlayerIndex];
        Invoke(nameof(GetControlOfACharacter), 0.5f);
    }

    public void GetControlOfACharacter()
    {
        ActivePlayer.MarkPlayerAsPlayable();
        _playerCamera.SwitchPlayer();
        _playerCamera.FadeCameraOutShort();
    }
}