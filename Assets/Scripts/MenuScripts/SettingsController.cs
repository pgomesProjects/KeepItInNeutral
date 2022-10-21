using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Toggle vSyncToggle;

    public void RefreshMenu()
    {
        //Update the values of the sliders and toggle so that they reflect the stored values
        musicSlider.value = PlayerPrefs.GetFloat("BGMVolume") / 0.1f;
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume") / 0.1f;

        switch (PlayerPrefs.GetInt("VSyncEnabled", 1))
        {
            case 0:
                vSyncToggle.isOn = false;
                break;
            case 1:
                vSyncToggle.isOn = true;
                break;
        }
    }

    public void ChangeBGMVolume(float value)
    {
        PlayerPrefs.SetFloat("BGMVolume", value * 0.1f);
        Debug.Log("BGM Volume: " + PlayerPrefs.GetFloat("BGMVolume", 0.5f));

        //Change the background music volume
        if(SceneManager.GetActiveScene().name == "Titlescreen")
            FindObjectOfType<AudioManager>().ChangeVolume("MainMenuOST", PlayerPrefs.GetFloat("BGMVolume"));
        else
            FindObjectOfType<AudioManager>().ChangeVolume("MainMenuOST", PlayerPrefs.GetFloat("BGMVolume"));
    }

    public void ChangeSFXVolume(float value)
    {
        PlayerPrefs.SetFloat("SFXVolume", value * 0.1f);
        Debug.Log("SFX Volume: " + PlayerPrefs.GetFloat("SFXVolume", 0.5f));
    }

    public void VSyncEnabledToggle(bool isVSyncEnabled)
    {
        //Set the VSync toggle (VSync helps with screen tearing)
        if (isVSyncEnabled)
        {
            Debug.Log("VSync Enabled");
            PlayerPrefs.SetInt("VSyncEnabled", 1);
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            Debug.Log("VSync Disabled");
            PlayerPrefs.SetInt("VSyncEnabled", 0);
            QualitySettings.vSyncCount = 0;
        }
    }
}
