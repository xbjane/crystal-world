using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private string createdTag;
    private void Awake()
    {
        audioSource.Play();
        //StartCoroutine(PlayMusic());
        //DontDestroyOnLoad(this.gameObject);
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
    private void LateUpdate() //дабы не создавалось множество объектов с музыкой при возврате на сцену
    {
       
    }
}
