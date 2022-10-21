using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    [SerializeField] private string selectSFXName, clickSFXName;

    public void SoundOnSelect()
    {
        //Play a sound effect when a button is selected
        if(selectSFXName != "")
        {
            FindObjectOfType<AudioManager>().PlayOneShot(selectSFXName, PlayerPrefs.GetFloat("SFXVolume", 0.5f));
        }
    }

    public void SoundOnClick()
    {
        //Play a sound effect when a button is clicked
        if(clickSFXName != "")
        {
            FindObjectOfType<AudioManager>().PlayOneShot(clickSFXName, PlayerPrefs.GetFloat("SFXVolume", 0.5f));
        }
    }
}
