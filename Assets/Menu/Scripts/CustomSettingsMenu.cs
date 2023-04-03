using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class CustomSettingsMenu : MonoBehaviour
{
    public AudioMixer MusicMixer;
    public AudioMixer EffectsMixer;

    private AudioSource _sampleSound;
    private CustomSettings _customSettings;
    private Text _musicVolumeText;
    private Text _effectsVolumeText;
    private Dropdown _resolutionsDropdown;
    private Slider _musicSlider;
    private Slider _effectsSlider;

    private List<Resolution> _availableResolutions;

    public void Awake()
    {
        _sampleSound = FindObjectsOfType<AudioSource>().Where(x => x.name.Equals("SampleSound")).First();
        _resolutionsDropdown = GetComponentInChildren<Dropdown>();
        _customSettings = CustomSettingsSaveLoadManager.GetCustomSettings();
        _availableResolutions = Screen.resolutions.ToList();

        GetTexts();
        GetSliders();
        SetUpMusicAndEffects();
        SetUpScreenResolutions();
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
        _musicSlider.value = _customSettings.MusicVolumeValue;

        _effectsSlider.value = _customSettings.EffectsVolumeValue;

        _musicVolumeText.text = $"Громкость музыки: {_customSettings.MusicVolumeValue + 50}";
        _effectsVolumeText.text = $"Громкость эффектов: {_customSettings.EffectsVolumeValue + 50}";
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
    }

    public void ChangeMusicVolume()
    {
        var currentMusicVolume = _musicSlider.value;
        MusicMixer.SetFloat(Constants.AudioVolume, currentMusicVolume);

        _musicVolumeText.text = $"Громкость музыки: {currentMusicVolume + 50}";
    }

    public void ChangeEffectsVolume()
    {
        var currentEffectsVolume = _effectsSlider.value;
        EffectsMixer.SetFloat(Constants.EffectsVolume, currentEffectsVolume);

        _effectsVolumeText.text = $"Громкость эффектов: {currentEffectsVolume + 50}";

        _sampleSound.Play();
    }

    public void ChangeResolution()
    {
        _customSettings.ScreenWidthValue = _availableResolutions[_resolutionsDropdown.value].width;
        _customSettings.ScreenHeightValue = _availableResolutions[_resolutionsDropdown.value].height;
        _customSettings.RefreshRate = _availableResolutions[_resolutionsDropdown.value].refreshRate;

        Screen.SetResolution(_customSettings.ScreenWidthValue, _customSettings.ScreenHeightValue, true, _customSettings.RefreshRate);
    }

    public void SaveSettings()
    {
        MusicMixer.GetFloat(Constants.AudioVolume, out _customSettings.MusicVolumeValue);
        EffectsMixer.GetFloat(Constants.EffectsVolume, out _customSettings.EffectsVolumeValue);
        _customSettings.ScreenWidthValue = _availableResolutions[_resolutionsDropdown.value].width;
        _customSettings.ScreenHeightValue = _availableResolutions[_resolutionsDropdown.value].height;
        _customSettings.RefreshRate = _availableResolutions[_resolutionsDropdown.value].refreshRate;

        CustomSettingsSaveLoadManager.SaveCustomSettings(_customSettings);
    }
}

