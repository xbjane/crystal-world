using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpWood : MonoBehaviour
{
  private void OnCollisionEnter2D(Collision2D collision) //вызывается, когда коллайдер другого объекта вступает с коллайдером этого объекта
  {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamage();
        }
    }
}
