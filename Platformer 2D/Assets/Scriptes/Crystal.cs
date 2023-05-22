using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public int count;
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Hero")
        {
            collision.gameObject.GetComponent<ScoreCount>().AddCrystal(count);
            Destroy(gameObject);
        }

    }
    
}
