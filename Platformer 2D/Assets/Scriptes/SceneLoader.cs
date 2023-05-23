using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public enum Scenes
    {
        menu,
        game
    }
    public static void Load(Scenes scene)
    {
        SceneManager.LoadScene(Convert.ToInt32(scene));
    }
}
