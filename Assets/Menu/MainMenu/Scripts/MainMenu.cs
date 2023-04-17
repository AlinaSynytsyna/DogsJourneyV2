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
    public SaveLoadSystem SaveLoadSystem;
    private ScreenFader _screenFader;

    public void Awake()
    {
        SetUpCustomSettings();
        SetUpCustomInputKeys();

        _screenFader = FindObjectOfType<ScreenFader>();
        _playButtonText = GetComponentsInChildren<Text>().Where(x => x.text.Equals("Start")).First();

        _playButtonText.text = GetPlayButtonText();
        FadeOut();
    }

    public void Update()
    {
        _playButtonText.text = GetPlayButtonText();
    }

    private void SetUpCustomSettings()
    {
        var _customSettings = CustomSettingsManager.GetCustomSettings();

        Screen.SetResolution(_customSettings.ScreenWidthValue, _customSettings.ScreenHeightValue, true, _customSettings.RefreshRate);
        MusicMixer.SetFloat(Constants.AudioVolume, _customSettings.MusicVolumeValue);
        EffectsMixer.SetFloat(Constants.EffectsVolume, _customSettings.EffectsVolumeValue);

        //var mainMenuCanvas = GetComponentInParent<CanvasScaler>();
        //mainMenuCanvas.referenceResolution = new Vector2(_customSettings.ScreenWidthValue, _customSettings.ScreenHeightValue);
    }

    private void SetUpCustomInputKeys()
    {
        var _customInput = CustomInputManager.GetCustomInputKeys();
        CustomInputManager.SaveCustomInputKeys(_customInput);
    }

    private string GetPlayButtonText()
    {
        return SaveLoadSystem.HasInfo ? "Продолжить" : "Новая игра";
    }

    public void PlayButtonPressed()
    {
        FadeIn();
        Invoke(nameof(Play), 2F);
    }

    private void Play()
    {
        var _sceneIndex = SaveLoadSystem.HasInfo ? SaveLoadSystem.CurrentSceneIndex : SceneManager.GetActiveScene().buildIndex + 1;

        SceneManager.LoadScene(_sceneIndex);
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