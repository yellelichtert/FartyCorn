using System;
using UnityEngine;
using UnityEngine.UIElements;

public static class SettingsManager
{
    
    public static void SetUserSettings()
    {
        AudioListener.pause = !IsSoundEnabled();
    }
    
    public static bool IsSoundEnabled() =>  PlayerPrefs.GetInt("isSoundEnabled", 1) == 1;
    public static void ToggleSound(bool isEnabled)
    {
        PlayerPrefs.SetInt("isSoundEnabled", Convert.ToInt32(isEnabled));
        AudioListener.pause = !isEnabled;
    }
    
    
    
    public static bool IsLowPowerMode() => PlayerPrefs.GetInt("isLowPowerMode", 0) == 1;
    public static void ToggleLowPowerMode(bool isEnabled)
    {
        PlayerPrefs.SetInt("isLowPowerMode", Convert.ToInt32(isEnabled));
    }
        
    
}