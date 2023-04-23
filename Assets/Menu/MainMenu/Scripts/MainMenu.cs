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

    public void Awake()
    {
        LevelManager.GetLevelInfo();
        SetUpCustomSettings();
        SetUpCustomInputKeys();

        _screenFader = FindObjectOfType<ScreenFader>();
        _playButtonText = GetComponentsInChildren<Text>().Where(x => x.text.Equals("Start")).First();

        GetPlayButtonText();
        FadeOut();
    }

    private void SetUpCustomSettings()
    {
        var _customSettings = CustomSettingsManager.GetCustomSettings();

        Screen.SetResolution(_customSettings.ScreenWidthValue, _customSettings.ScreenHeightValue, true, _customSettings.RefreshRate);
        MusicMixer.SetFloat(Constants.AudioVolume, _customSettings.MusicVolumeValue);
        EffectsMixer.SetFloat(Constants.EffectsVolume, _customSettings.EffectsVolumeValue);
    }

    private void SetUpCustomInputKeys()
    {
        var _customInput = CustomInputManager.GetCustomInputKeys();
        CustomInputManager.SaveCustomInputKeys(_customInput);
    }

    public void GetPlayButtonText()
    {
        _playButtonText.text = LevelManager.HasInformation ? "Продолжить" : "Новая игра";
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
}