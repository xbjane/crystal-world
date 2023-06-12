using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsSettings : MonoBehaviour
{
    public SettingsData[] sD;
    public ToggleSwitch[] Toggles;
    public static PlayerPrefsSettings Instance;
    // private SettingsInfo settingsInfo;
    void Awake()
    {
        Instance = this;
        for(int i=0;i<sD.Length;i++)
        {
            if (PlayerPrefs.HasKey(sD[i].nameOfPref))
            {
                sD[i].play = PlayerPrefs.GetInt(sD[i].nameOfPref) == 1 ? true : false;
                Toggles[i].IsOn = sD[i].play;
            }
            else
            {
                sD[i].play = false;
            }
            Debug.Log(sD[i].nameOfPref + " Awake");
           // Toggle = Toggle;
            // s.Toggle.GetComponent<ToggleSwitch>();
            Toggles[i].valueChanged += SaveToPrefs;

        }
    }
       // Instance = this;
    //    if (PlayerPrefs.HasKey(nameOfPref))
    //    {
    //        SettingsInfo.playSFX = PlayerPrefs.GetInt(nameOfPref) == 1 ? true : false;
    //        Toggle.IsOn = SettingsInfo.playSFX;
    //    }
    //    else
    //    {
    //        SettingsInfo.playSFX = false;
    //    }
    //    Toggle.GetComponent<ToggleSwitch>();
    //    Toggle.valueChanged += SaveToPrefs;

    //}
    public void SaveToPrefs(ToggleSwitch toggle, bool value)
    {

        for(int i = 0; i < sD.Length; i++)
        {
            if (toggle.gameObject.name == sD[i].nameOfPref)
            {
                Debug.Log("=");
                sD[i].play = value;
                PlayerPrefs.SetInt(sD[i].nameOfPref, value ? 1 : 0);
            }
            else Debug.Log(toggle.gameObject.name + "!=" + sD[i].nameOfPref);
        }
        //if (this.Toggle.gameObject.name == "SFXToggle")
        //{
        //    SettingsInfo.playSFX = value;
        //}
        //else if (this.Toggle.gameObject.name == "MusicToggle")
        //{
        //    SettingsInfo.playMusic = value;
        //}
        //else if (this.Toggle.gameObject.name == "VibrationToggle")
        //{
        //    SettingsInfo.playMusic = value;
        //}
       

    }
   

}
