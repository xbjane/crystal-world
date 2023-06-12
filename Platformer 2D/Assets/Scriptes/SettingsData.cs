using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SettingsData", menuName = "CreateSettingsData")]
public class SettingsData : ScriptableObject
{
    [SerializeField]
    public bool play;
    [SerializeField]
    public string nameOfPref;
}
