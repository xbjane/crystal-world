using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour //скрипт для начлежования всем монстрам
{
   public virtual void GetDamage() { }
    public virtual void Die()
    {
        Destroy(this.gameObject);
    }
  
}
