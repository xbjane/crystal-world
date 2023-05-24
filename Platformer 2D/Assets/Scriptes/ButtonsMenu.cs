using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsMenu: MonoBehaviour
{
    public Canvas scoreCanvas;
    private void Start()
    {
        scoreCanvas.gameObject.SetActive(false);
    }
    public static void StartGame() 
   {
        SceneLoader.Load(SceneLoader.Scenes.game);
   }
    public void FinishGame()
    {
        SceneLoader.Load(SceneLoader.Scenes.menu);
    }
    public void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
