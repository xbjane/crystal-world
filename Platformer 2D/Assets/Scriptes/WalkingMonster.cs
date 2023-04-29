using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingMonster : Entity
{
    private float speed = 3.5f;
    private Animator anim;
    private Vector3 dir;
    private SpriteRenderer sprite;
    private StatesOfMonster State
    {
        get { return (StatesOfMonster)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }
    }
    private void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>(true);
    }
    private void Start()
    {
        dir = transform.right;
    }
    private void Update()
    {
        if (CheckLets())
            State = StatesOfMonster.idle;
        else
        {
            State = StatesOfMonster.walk;
            Move();
        }
    }
    private bool CheckLets()
    {
        if (Physics2D.OverlapCircleAll(transform.position + transform.up * 0.1f + transform.right * dir.x * 0.7f, 0.1f).Length != 0 && Physics2D.OverlapCircleAll(transform.position + transform.up * 0.1f + transform.right * (-dir.x) * 0.7f, 0.1f).Length != 0)
            return true;
        return false;
    }
    private void Move()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.1f + transform.right * dir.x * 0.7f, 0.1f);//возвращает массив коллайдеров, находящийся вокруг указанной точки в указанном радиусе
        if (colliders.Length > 0)
        {
            dir.x *= -1f; //меняем направление движения на противоположное          
        }
        transform.position = Vector3.MoveTowards(transform.position, transform.position+dir, Time.deltaTime);//перемещение врага
        sprite.flipX = dir.x > 0.0f; //поворот монстра(переключение галочки)
    }
    private void OnCollisionEnter2D(Collision2D collision) //вызывается, когда коллайдер другого объекта вступает с коллайдером этого объекта
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamage();
        }
    }
}
public enum StatesOfMonster
{
    idle, 
    walk
}
