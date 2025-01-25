using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public TMP_Dropdown graphicsDropdown, windowDropdown;
    public Slider MasterVolumeSlider, MusicVolumeSlider, SFXVolumeSlider;
    public AudioMixer audioMixer;


    public void SetGraphicsQuality()
    {
        QualitySettings.SetQualityLevel(graphicsDropdown.value);
    }

    public void SetWindowMode()
    {
        switch (windowDropdown.value)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
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
}
