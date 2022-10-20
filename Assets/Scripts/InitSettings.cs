using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitSettings : MonoBehaviour
{
    private void Awake()
    {
        InitializeSettings();
    }

    private void InitializeSettings()
    {
        switch(PlayerPrefs.GetInt("VSyncEnabled", 1))
        {
            case 0:
                Debug.Log("VSync Disabled");
                QualitySettings.vSyncCount = 0;
                break;
            case 1:
                Debug.Log("VSync Enabled");
                QualitySettings.vSyncCount = 1;
                break;
        }
    }
}
