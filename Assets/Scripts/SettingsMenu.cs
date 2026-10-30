using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public TMP_Dropdown graphicsDropdown, windowDropdown;
    public Slider MasterVolumeSlider, MusicVolumeSlider, SFXVolumeSlider, AmbienceVolumeSlider;
    public AudioMixer audioMixer;

    private void OnEnable()
    {
        graphicsDropdown.SetValueWithoutNotify(QualitySettings.GetQualityLevel());

        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.Windowed:
                windowDropdown.SetValueWithoutNotify(0);
                break;
            case FullScreenMode.ExclusiveFullScreen:
                windowDropdown.SetValueWithoutNotify(2);
                break;
            default:
                windowDropdown.SetValueWithoutNotify(1);
                break;
        }

        ShowMixerVolume(MasterVolumeSlider, "Master");
        ShowMixerVolume(SFXVolumeSlider, "SFX");
        ShowMixerVolume(MusicVolumeSlider, "Music");
        ShowMixerVolume(AmbienceVolumeSlider, "Ambience");
    }

    private void ShowMixerVolume(Slider slider, string parameter)
    {
        if (audioMixer.GetFloat(parameter, out float volume))
        {
            slider.SetValueWithoutNotify(volume);
        }
    }

    public void SetGraphicsQuality()
    {
        QualitySettings.SetQualityLevel(graphicsDropdown.value);
    }

    public void SetWindowMode()
    {
        int monitorWidth = Display.main.systemWidth;
        int monitorHeight = Display.main.systemHeight;

        switch (windowDropdown.value)
        {
            case 0:
                Screen.SetResolution(Mathf.Min(1280, monitorWidth), Mathf.Min(720, monitorHeight), FullScreenMode.Windowed);
                break;
            case 1:
                Screen.SetResolution(monitorWidth, monitorHeight, FullScreenMode.FullScreenWindow);
                break;
            case 2:
                Screen.SetResolution(monitorWidth, monitorHeight, FullScreenMode.ExclusiveFullScreen);
                break;
        }
    }

    public void ChangeMasterVolume()
    {
        audioMixer.SetFloat("Master", MasterVolumeSlider.value);
    }

    public void ChangeSFXVolume()
    {
        audioMixer.SetFloat("SFX", SFXVolumeSlider.value);
    }

    public void ChangeMusicVolume()
    {
        audioMixer.SetFloat("Music", MusicVolumeSlider.value);
    }

    public void ChangeAmbienceVolume()
    {
        audioMixer.SetFloat("Ambience", AmbienceVolumeSlider.value);
    }
}
