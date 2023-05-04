using Knot.Localization;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public AudioMixer MusicMixer;
    public AudioMixer EffectsMixer;
    private Text _playButtonText;
    private ScreenFader _screenFader;
    private CustomSettings _customSettings;
    private string _playButtonTextString;

    public void Awake()
    {
        _customSettings = CustomSettingsManager.GetCustomSettings();

        LevelManager.GetLevelInfo();

        SetUpLanguage();
        SetUpCustomSettings();
        SetUpCustomInputKeys();

        _screenFader = FindObjectOfType<ScreenFader>();
        _playButtonText = GetComponentsInChildren<Text>().Where(x => x.text.Equals("Start")).First();

        GetPlayButtonText();
        FadeOut();

        KnotLocalization.RegisterTextUpdatedCallback(LocalizationConstants.ContinueGameLabel, MainMenuLabelUpdated);
        KnotLocalization.RegisterTextUpdatedCallback(LocalizationConstants.NewGameLabel, MainMenuLabelUpdated);
    }

    private void SetUpCustomSettings()
    {
        Screen.SetResolution(_customSettings.ScreenWidthValue, _customSettings.ScreenHeightValue, true, _customSettings.RefreshRate);
        MusicMixer.SetFloat(Constants.AudioVolume, _customSettings.MusicVolumeValue);
        EffectsMixer.SetFloat(Constants.EffectsVolume, _customSettings.EffectsVolumeValue);
    }

    private void SetUpCustomInputKeys()
    {
        var _customInput = CustomInputManager.GetCustomInputKeys();
        CustomInputManager.SaveCustomInputKeys(_customInput);
    }

    private void SetUpLanguage()
    {
        var targetLanguage = KnotLocalization.Manager.Languages.FirstOrDefault(d => (int)d.SystemLanguage == _customSettings.SystemLanguage);

        if (targetLanguage != null)
            KnotLocalization.Manager.LoadLanguage(targetLanguage);
    }

    public void GetPlayButtonText()
    {
        _playButtonText.text = LevelManager.HasInformation ? KnotLocalization.GetText(LocalizationConstants.ContinueGameLabel) : KnotLocalization.GetText(LocalizationConstants.NewGameLabel);
    }

    public void PlayButtonPressed()
    {
        FadeIn();
        Invoke(nameof(Play), 2F);
    }

    private void Play()
    {
        LevelManager.IsReloadingLevel = false;
        try
        {
            SceneManager.LoadScene(LevelManager.GetLevelIndex());
        }
        catch
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void ExitButttonPressed()
    {
        FadeIn();
        Invoke(nameof(Exit), 2F);
    }

    private void Exit()
    {
        Application.Quit();
    }

    public void FadeOut()
    {
        _screenFader.fromOutDelay = Constants.FadeDelay;
        _screenFader.fadeSpeed = Constants.FadeSpeed;
        _screenFader.fadeState = ScreenFader.FadeState.Out;
    }

    public void FadeIn()
    {
        _screenFader.fromInDelay = Constants.FadeDelay;
        _screenFader.fadeSpeed = Constants.FadeSpeed;
        _screenFader.fadeState = ScreenFader.FadeState.In;
    }

    private void MainMenuLabelUpdated(string text)
    {
        if(!_playButtonText.IsDestroyed())
        _playButtonText.text = text;
    }
}