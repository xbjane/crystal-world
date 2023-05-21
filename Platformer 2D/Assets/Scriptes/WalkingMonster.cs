using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingMonster : Entity
{
    public float attackRadius;
    [SerializeField] private float speed = 3.5f;
    //private Animator anim;
    private Vector3 dir;
    private SpriteRenderer sprite;
    private Animator anim;
  //  public static WalkingMonster Instance { get; set; }
    private States State
    {
        get { return (States)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }
    }
    private void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
       // Instance = this;
    }
    private void Start()
    {
        dir = transform.right;
        lives = 3;
    }
    private void Update()
    {
        if (State != States.hit)
        {
            if (CheckLets())
                State = States.idle;
            else
            {
                State = States.walk;
                Move();
            }
        }
    }
    private bool CheckLets()
    {
        if (Physics2D.OverlapCircleAll(transform.position/* + transform.up * 0.5f*/ + transform.right * dir.x * 0.1f, 0.2f).Length > 1 && Physics2D.OverlapCircleAll(transform.position /*+ *//*transform.up * 0.1f*/ + transform.right * (-dir.x) * 0.1f, 0.2f).Length > 1)
            return true;
        return false;
    }
    private void Move()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position /*+ transform.up * 0.1f*/ + transform.right * dir.x * 0.1f, 0.2f);//возвращает массив коллайдеров, находящийся вокруг указанной точки в указанном радиусе
        if (colliders.Length > 1)
        {
            bool isHero = false;
            foreach (Collider2D c in colliders)
            {
                if (c.gameObject == Hero.Instance.gameObject)
                {
                    isHero = true;
                     State = States.attack;
                    //StartCoroutine(AttackHero());
                    break;
                }
            }
                if(!isHero)
            dir.x *= -1f; //меняем направление движения на противоположное          
        }
        Debug.Log(dir.x);
        transform.position = Vector3.MoveTowards(transform.position, transform.position+dir, speed*Time.deltaTime);//перемещение врага
        sprite.flipX = dir.x < 0.0f; //поворот монстра(переключение галочки)
    }
    public override void GetDamage()
    {
        lives--;
        State = States.hit;
        if (lives < 1)
            Die();
    }
    private IEnumerator AttackHero()
    {
        yield return new WaitForSeconds(0.4f);
    }
    //private void OnCollisionEnter2D(Collision2D collision) //вызывается, когда коллайдер другого объекта вступает с коллайдером этого объекта
    //{
    //    //if (collision.gameObject == Hero.Instance.gameObject)
    //    //{
    //    //    //if ((Hero.Instance.gameObject.transform.position.x < transform.position.x && dir.x < 0.0f) || (Hero.Instance.gameObject.transform.position.x > transform.position.x && dir.x > 0.0f))
    //    //       /// State = StatesOfMonster.attack;
    //    //   // Debug.Log("OnCollisionEnter2D");
    //    //    //StartCoroutine(AttackAnimation());
    //    //    Hero.Instance.GetDamage();
    //    //}
    //    //State = StatesOfMonster.attack;
    //}
    private void OnAttack()
    {
        //Debug.Log("OnAttack");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRadius);
        foreach (Collider2D collider in colliders)
        {
            if(!Hero.Instance.isHit)
            Hero.Instance.GetDamage();
            return;
        }
    }
    //private IEnumerator AttackAnimation()
    //{
    //   // yield return new WaitForSeconds(0.25f);
    //}
    private enum States
    {
        idle,
        walk,
        attack, 
        hit
    }
}

