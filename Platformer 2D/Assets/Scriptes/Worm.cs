using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : Entity
{
    [SerializeField] private int lives = 3; //сериализация приватной переменной для отображения в редакторе
    private void OnCollisionEnter2D(Collision2D collision) //вызывается, когда коллайдер другого объекта вступает с коллайдером этого объекта
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamage();
            lives--;
            Debug.Log("У червячка" + lives);
        }
        if (lives < 1)
            Die();
    }

}
