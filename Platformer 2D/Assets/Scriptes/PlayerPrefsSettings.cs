using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsSettings : MonoBehaviour
{
    public SettingsData[] sD;
    public ToggleSwitch[] Toggles;
    public static PlayerPrefsSettings Instance;
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
            Toggles[i].valueChanged += SaveToPrefs;

        }
    }
    public void SaveToPrefs(ToggleSwitch toggle, bool value)
    {

        for(int i = 0; i < sD.Length; i++)
        {
            if (toggle.gameObject.name == sD[i].nameOfPref)
            {
                sD[i].play = value;
                PlayerPrefs.SetInt(sD[i].nameOfPref, value ? 1 : 0);
            }
        }
    }
}
