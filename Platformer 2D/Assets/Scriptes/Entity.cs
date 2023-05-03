using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour //скрипт для начлежования всем монстрам
{
    protected int lives;
   public virtual void GetDamage() 
    {
        lives--;
        if (lives < 1)
            Die();
    }
    public virtual void Die()
    {
        Destroy(this.gameObject);
    }
  
}
