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

        LoadProgress();

        if (!LevelManager.IsReloadingLevel)
        {
            GameObject.FindObjectOfType<DialogueRunner>().LoadStateFromPlayerPrefs(Constants.SaveKey);
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

    public static void SaveProgress()
    {
        GameObject.FindObjectOfType<DialogueRunner>().SaveStateToPlayerPrefs(Constants.SaveKey);
        LevelManager.SavePlayers();
        HealingObjectsManager.SaveHealingObjects();
        DialogueTriggersManager.SaveDialogueTriggers();
        TutorialTriggersManager.SaveTutorialTriggers();
        TeleportTriggersManager.SaveTeleportTriggers();
        LevelManager.SaveLevelIndex();
        LevelManager.SaveLevelInfo();
    }

    public void LoadProgress()
    {
        LevelManager.GetLevelInfo();
        LevelManager.CLearPlayerPrefsIfSaveFileIsDeleted();
        HealingObjectsManager.GetAllHealingObjects();
        TeleportTriggersManager.LoadAllTeleportTriggers();
        TutorialTriggersManager.LoadAllTutorialTriggers();
        DialogueTriggersManager.LoadAllDialogueTriggers();
    }
}