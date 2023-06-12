using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private string createdTag;
    private bool playMusiclocal;

    private void Awake()
    {
        if (PlayerPrefsSettings.Instance.sD[1].play)
        audioSource.Play();
        playMusiclocal = PlayerPrefsSettings.Instance.sD[1].play;
        GameObject gO = GameObject.FindWithTag(this.createdTag);
        if (gO != null)
        {
             Destroy(this.gameObject);
        }
        else
        {
            this.gameObject.tag = this.createdTag;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    private IEnumerator PlayMusic()
    {

        yield return new WaitForSeconds(audioSource.clip.length);
    }
    private void LateUpdate()
    {
        if (playMusiclocal != PlayerPrefsSettings.Instance.sD[1].play)
        {
            playMusiclocal = PlayerPrefsSettings.Instance.sD[1].play;
            if(playMusiclocal)
                audioSource.Play();
            else
            audioSource.Stop();

        }

    }
}
