using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPanelController : MonoBehaviour
{
    public Slider sfxSlider;
    public Slider musicSlider;

    void Start()
    {
        // Initialize sliders with current values
        sfxSlider.value = AudioManager.instance.GetSFXVolume();
        musicSlider.value = AudioManager.instance.GetMusicVolume();

        sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
        musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
    }


    public void UpdateSFXVolume(float volume)
    {
        AudioManager.instance.SetSFXVolume(volume);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    public void UpdateMusicVolume(float volume)
    {
        AudioManager.instance.SetMusicVolume(volume);
        PlayerPrefs.SetFloat("bgMusicVolume", volume);
    }

    public void UpdateSoundSettings()
    {
        float sfxVolume = sfxSlider.value;
        float bgMusicVolume = musicSlider.value;

        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        PlayerPrefs.SetFloat("bgMusicVolume", bgMusicVolume);

        AudioManager.instance.UpdateVolume(sfxVolume, bgMusicVolume);

        UpdateSliders();
    }

    public void UpdateSliders()
    {
        sfxSlider.value = AudioManager.instance.GetSFXVolume();
        musicSlider.value = AudioManager.instance.GetMusicVolume();
    }

}

