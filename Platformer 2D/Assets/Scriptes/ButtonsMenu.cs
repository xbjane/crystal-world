using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsMenu: MonoBehaviour
{
    public Canvas scoreCanvas;
    [SerializeField] AudioSource audioSource;
   // [SerializeField] AudioClip audioClip;
    //public void PlaySound(AudioClip audioClip)
    //{
    //    if (SettingsInfo.playSFX)
    //        audioSource.Play()/*OneShot(audioClip)*/;
    //    else Debug.Log("No Sound");
    //}
    private void Start()
    {
        scoreCanvas.gameObject.SetActive(false);
       // audioSource = GetComponent<AudioSource>();
    }
    public void StartGame() 
   {
        StartCoroutine(PlaySound());      
   }
    private IEnumerator PlaySound()
    {
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length-0.3f);
        SceneLoader.Load(SceneLoader.Scenes.game);
    }
    public void FinishGame()
    {
        SceneLoader.Load(SceneLoader.Scenes.menu);
    }
    public void Exit()
    {
        //if (Settings.playSFX)
        //    audioSource.PlayOneShot(audioClip);
        Debug.Log("Exit");
        Application.Quit();
    }
}
