using Knot.Localization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yarn.Unity;

public class LevelInfo : MonoBehaviour
{
    public List<Player> PlayableCharacters;
    public Player ActivePlayer;
    public string MainPlayableCharacter;

    private CustomInput _customInput;
    private PlayerCamera _playerCamera;

    public void Awake()
    {
        _customInput = CustomInputManager.GetCustomInputKeys();
        _playerCamera = FindObjectOfType<PlayerCamera>();
        LevelManager.GetLevelInfo();
        GameObject.FindObjectOfType<DialogueRunner>().LoadStateFromPlayerPrefs(Constants.SaveKey);
        if (!LevelManager.IsReloadingLevel)
        {
            HealingObjectsManager.GetAllHealingObjects();
            DialogueTriggersManager.LoadAllDialogueTriggers();
        }
    }

    public void Start()
    {
        var targetLanguage = KnotLocalization.Manager.Languages.FirstOrDefault(d => (int)d.SystemLanguage == CustomSettingsManager.GetCustomSettings().SystemLanguage);

        if (targetLanguage != null)
        {
            KnotLocalization.Manager.LoadLanguage(targetLanguage);
            FindObjectOfType<TextLineProvider>().textLanguageCode = targetLanguage.CultureName;
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
        return PlayableCharacters.Contains(playerName) && playerName.IsPlayableInScene;
    }

    public void SwitchCharacter()
    {
        var activePlayerIndex = PlayableCharacters.IndexOf(ActivePlayer) + 1 != PlayableCharacters.Count ? PlayableCharacters.IndexOf(ActivePlayer) + 1 : 0;

        if (PlayableCharacters[activePlayerIndex].IsPlayableInScene)
        {
            _playerCamera.FadeCameraInShort();
            ActivePlayer.MarkPlayerAsUnplayable();
            ActivePlayer = PlayableCharacters[activePlayerIndex];
            Invoke(nameof(GetControlOfACharacter), 0.5f);
        }
    }

    public void GetControlOfACharacter()
    {
        ActivePlayer.MarkPlayerAsPlayable();
        _playerCamera.SwitchPlayer();
        _playerCamera.FadeCameraOutShort();
    }
}