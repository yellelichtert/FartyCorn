using System;
using UnityEngine;

public static class SettingsManager
{
    
    public static void SetUserSettings()
    {
        AudioListener.pause = !IsSoundEnabled();
    }
    
    public static bool IsSoundEnabled() =>  PlayerPrefs.GetInt(PlayerPrefKeys.Sound, 1) == 1;
    public static void ToggleSound(bool isEnabled)
    {
        PlayerPrefs.SetInt(PlayerPrefKeys.Sound, Convert.ToInt32(isEnabled));
        AudioListener.pause = !isEnabled;
    }
    
    
    public static bool IsLowPowerMode() => PlayerPrefs.GetInt(PlayerPrefKeys.LowPower, 0) == 1;
    public static void ToggleLowPowerMode(bool isEnabled)
    {
        PlayerPrefs.SetInt(PlayerPrefKeys.LowPower, Convert.ToInt32(isEnabled));
    }
}