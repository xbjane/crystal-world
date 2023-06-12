using System.Collections;
using UnityEngine;

public class ButtonsMenu: MonoBehaviour
{
    public Canvas scoreCanvas;
    [SerializeField] AudioSource audioSource;
    private void Start()
    {
        scoreCanvas.gameObject.SetActive(false);
    }
    public void PlaySoundFunc()
    {
        if(PlayerPrefsSettings.Instance.sD[0].play)
            audioSource.Play();
    }
    public void StartGame() 
   {
        StartCoroutine(PlaySound());      
   }
    private IEnumerator PlaySound()
    {
        if (PlayerPrefsSettings.Instance.sD[0].play)
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
        Application.Quit();
    }
}
