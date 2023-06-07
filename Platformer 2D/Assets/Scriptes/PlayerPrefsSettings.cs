using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsSettings : MonoBehaviour
{
    [SerializeField] string nameOfPref;
    public ToggleSwitch Toggle;
    public delegate void PrefValueChanged(bool value);
    public event PrefValueChanged prefValueChanged;
    public static PlayerPrefsSettings Instance;
    // private SettingsInfo settingsInfo;
    void Awake()
    {
        Instance = this;
        if (PlayerPrefs.HasKey(nameOfPref))
        {
            SettingsInfo.playSFX = PlayerPrefs.GetInt(nameOfPref) == 1 ? true : false;
            Toggle.IsOn = SettingsInfo.playSFX;
        }
        else
        {
            SettingsInfo.playSFX = false;
        }
        Toggle.GetComponent<ToggleSwitch>();
        Toggle.valueChanged += SaveToPrefs;

    }
    public void SaveToPrefs(bool value)
    {

        if (this.Toggle.gameObject.name == "SFXToggle")
        {
            SettingsInfo.playSFX = value;
        }
        else if (this.Toggle.gameObject.name == "MusicToggle")
        {
            SettingsInfo.playMusic = value;
        }
        else if (this.Toggle.gameObject.name == "VibrationToggle")
        {
            SettingsInfo.playMusic = value;
        }
        PlayerPrefs.SetInt(nameOfPref, value ? 1 : 0);
        prefValueChanged?.Invoke(value);

    }
   

}
