using Knot.Localization;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class CustomSettingsMenu : MonoBehaviour
{
    public AudioMixer MusicMixer;
    public AudioMixer EffectsMixer;
    public List<SystemLanguage> AvailableLanguages;

    private Button _deleteSaveButton;
    private AudioSource _sampleSound;
    private CustomSettings _customSettings;
    private Text _musicVolumeText;
    private Text _effectsVolumeText;
    private Dropdown _resolutionsDropdown;
    private Dropdown _languageDropdown;
    private Slider _musicSlider;
    private Slider _effectsSlider;
    private Toggle _fullscreenToggle;
    private int _selectedLanguage;

    private List<Resolution> _availableResolutions;

    public void Awake()
    {
        _deleteSaveButton = GetComponentsInChildren<Button>().Where(x => x.name.Equals("DeleteSaveButton")).First();
        _sampleSound = FindObjectsOfType<AudioSource>().Where(x => x.name.Equals("SampleSound")).First();
        _resolutionsDropdown = GetComponentsInChildren<Dropdown>().Where(x => x.name.Equals("Resolution")).First();
        _languageDropdown = GetComponentsInChildren<Dropdown>().Where(x => x.name.Equals("Language")).First();
        _fullscreenToggle = GetComponentInChildren<Toggle>();
        _customSettings = CustomSettingsManager.GetCustomSettings();
        _availableResolutions = Screen.resolutions.ToList();

        GetTexts();
        GetSliders();
        SetUpMusicAndEffects();
        SetUpScreenResolutions();
        SetUpLanguages();

        if (LevelManager.HasInformation)
            _deleteSaveButton.interactable = true;

        KnotLocalization.RegisterTextUpdatedCallback(LocalizationConstants.EffectsLabel, EffectsLabelUpdated);
        KnotLocalization.RegisterTextUpdatedCallback(LocalizationConstants.MusicLabel, MusicLabelUpdated);
    }

    private void GetTexts()
    {
        var allTexts = GetComponentsInChildren<Text>().ToList();

        _musicVolumeText = allTexts.Where(x => x.name.Equals("AudioVolumeText")).First();
        _effectsVolumeText = allTexts.Where(x => x.name.Equals("EffectsVolumeText")).First();
    }

    private void GetSliders()
    {
        var allSliders = GetComponentsInChildren<Slider>().ToList();

        _musicSlider = allSliders.Where(x => x.name.Equals("Music")).First();
        _effectsSlider = allSliders.Where(x => x.name.Equals("Effects")).First();
    }

    private void SetUpMusicAndEffects()
    {
        var effectsLabel = KnotLocalization.GetText(LocalizationConstants.EffectsLabel);
        var musicLabel = KnotLocalization.GetText(LocalizationConstants.MusicLabel);

        _musicSlider.value = _customSettings.MusicVolumeValue;

        _effectsSlider.value = _customSettings.EffectsVolumeValue;

        _musicVolumeText.text = $"{musicLabel}: {_customSettings.MusicVolumeValue + 50}";
        _effectsVolumeText.text = $"{effectsLabel}: {_customSettings.EffectsVolumeValue + 50}";
    }

    private void SetUpScreenResolutions()
    {
        var textResolutions = new List<string>();

        foreach (var resolution in _availableResolutions)
        {
            textResolutions.Add($"{resolution.width}x{resolution.height} {resolution.refreshRate} Hz");
        }

        _resolutionsDropdown.AddOptions(textResolutions);

        _resolutionsDropdown.value = textResolutions.IndexOf($"{_customSettings.ScreenWidthValue}x{_customSettings.ScreenHeightValue} {_customSettings.RefreshRate} Hz");

        _fullscreenToggle.isOn = _customSettings.IsFullscreen;
    }

    private void SetUpLanguages()
    {
        var textLanguages = new List<string>();

        foreach (var language in AvailableLanguages)
        {
            textLanguages.Add(language.ToString());
        }

        _languageDropdown.AddOptions(textLanguages);

        _selectedLanguage = _customSettings.SystemLanguage;
        _languageDropdown.value = textLanguages.IndexOf(AvailableLanguages.FirstOrDefault(x => (int)x == _selectedLanguage).ToString());
    }

    public void ChangeMusicVolume()
    {
        var musicLabel = KnotLocalization.GetText(LocalizationConstants.MusicLabel);
        var currentMusicVolume = _musicSlider.value;
        MusicMixer.SetFloat(Constants.AudioVolume, currentMusicVolume);

        _musicVolumeText.text = $"{musicLabel}: {currentMusicVolume + 50}";
    }

    public void ChangeEffectsVolume()
    {
        var effectsLabel = KnotLocalization.GetText(LocalizationConstants.EffectsLabel);
        var currentEffectsVolume = _effectsSlider.value;
        EffectsMixer.SetFloat(Constants.EffectsVolume, currentEffectsVolume);

        _effectsVolumeText.text = $"{effectsLabel}: {currentEffectsVolume + 50}";

        _sampleSound.Play();
    }

    public void ChangeResolution()
    {
        _customSettings.ScreenWidthValue = _availableResolutions[_resolutionsDropdown.value].width;
        _customSettings.ScreenHeightValue = _availableResolutions[_resolutionsDropdown.value].height;
        _customSettings.RefreshRate = _availableResolutions[_resolutionsDropdown.value].refreshRate;

        Screen.SetResolution(_customSettings.ScreenWidthValue, _customSettings.ScreenHeightValue, _customSettings.IsFullscreen, _customSettings.RefreshRate);
    }

    public void ChangeLanguage()
    {
        var targetLanguage = KnotLocalization.Manager.Languages.FirstOrDefault(d => d.SystemLanguage == AvailableLanguages[_languageDropdown.value]);
        if (targetLanguage != null)
            KnotLocalization.Manager.LoadLanguage(targetLanguage);

        _selectedLanguage = (int)AvailableLanguages[_languageDropdown.value];
    }

    public void ChangeFullscreen()
    {
        _customSettings.IsFullscreen = _fullscreenToggle.isOn;
        Screen.fullScreen = _fullscreenToggle.isOn;
    }

    public void DeleteSave()
    {
        LevelManager.DeleteSaveFile();
        LevelManager.LevelInfoJObject = new JObject();
        PlayerPrefs.DeleteAll();
        _deleteSaveButton.interactable = false;
    }

    public void SaveSettings()
    {
        MusicMixer.GetFloat(Constants.AudioVolume, out _customSettings.MusicVolumeValue);
        EffectsMixer.GetFloat(Constants.EffectsVolume, out _customSettings.EffectsVolumeValue);
        _customSettings.ScreenWidthValue = _availableResolutions[_resolutionsDropdown.value].width;
        _customSettings.ScreenHeightValue = _availableResolutions[_resolutionsDropdown.value].height;
        _customSettings.RefreshRate = _availableResolutions[_resolutionsDropdown.value].refreshRate;
        _customSettings.IsFullscreen = _fullscreenToggle.isOn;
        _customSettings.SystemLanguage = _selectedLanguage;

        CustomSettingsManager.SaveCustomSettings(_customSettings);
    }

    void EffectsLabelUpdated(string text)
    {
        if (!_effectsSlider.IsDestroyed())
            _effectsVolumeText.text = $"{text}: {_effectsSlider.value + 50}";
    }

    void MusicLabelUpdated(string text)
    {
        if (!_musicSlider.IsDestroyed())
            _musicVolumeText.text = $"{text}: {_musicSlider.value + 50}";
    }
}

