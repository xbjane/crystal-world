using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : /*Enemy*/ Entity
{
    public float attackRadius;
    public float pivotRightDist;
    public float radiusOfLets;
    [SerializeField] private float speed;
    private Vector3 dir;
    private SpriteRenderer sprite;
    private Animator anim;
    private bool isHit;
    private bool isALet;
    //private bool isHero;
    private Collider2D colliderOfHero;
    private States State
    {
        get { return (States)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }
    }
    private void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        isHit = false;
        isALet = false;
        colliderOfHero = null;
        //isHero = false;
    }
    private void Start()
    {
        dir = transform.right;
        lives = 3;
    }
    private void Update()
    {
        if (!isHit)
        {
            //if (CheckLets())
            //    State = States.idle;
            //else
            //{
            //    State = States.walk;
            //    Move();
            //}
            Behavior();
        }
    }
    private void Behavior()
    {
        Collider2D[] rightColliders = Physics2D.OverlapCircleAll(transform.position/* + transform.up * 0.5f*/ + transform.right * dir.x * pivotRightDist, radiusOfLets);
        if (rightColliders.Length > 1)
        {
            bool isHero = false;
            foreach (Collider2D c in rightColliders)
            {
                if (c.gameObject == Hero.Instance.gameObject && !Hero.Instance.isDead)
                {
                    isHero = true;
                    State = States.attack;
                    return;
                }
            }
            if (Hero.Instance.isDead)
            {
                State = States.idle;
                return;
            }
            if (!isHero)
            {
                dir.x *= -1f;
                sprite.flipX =dir.x<0.0f;
                Collider2D[] leftColliders = Physics2D.OverlapCircleAll(transform.position/* + transform.up * 0.5f*/ + transform.right * dir.x * pivotRightDist, radiusOfLets);
                if (leftColliders.Length > 1)
                {
                    foreach (Collider2D c in leftColliders)
                    {
                        if (c.gameObject == Hero.Instance.gameObject && !Hero.Instance.isDead)
                        {
                            isHero = true;
                            State = States.attack;
                            return;
                        }
                    }
                    if (!isHero|| Hero.Instance.isDead)
                    {
                        State = States.idle;
                        return;
                    }
                }
            }
        }
        State = States.walk;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);//перемещение врага
    }
    //private bool CheckLets()
    //{
    //    List<Collider2D> rightColliders = new List<Collider2D>();
    //    rightColliders.AddRange(Physics2D.OverlapCircleAll(transform.position/* + transform.up * 0.5f*/ + transform.right * dir.x * pivotRightDist, radiusOfLets));
    //    if (rightColliders.Count > 1)
    //    {
    //        foreach (Collider2D c in rightColliders)
    //        {
    //            Debug.Log(c.name);
    //            if (c.gameObject == Hero.Instance.gameObject && !Hero.Instance.isDead)
    //            {
    //                State = States.attack;
    //                return false;
    //            }
    //        }
    //        isALet = true;
    //    }
    //    //colliders.Clear();
    //    List<Collider2D> leftColliders = new List<Collider2D>();
    //    leftColliders.AddRange(Physics2D.OverlapCircleAll(transform.position /*+ *//*transform.up * 0.1f*/ + transform.right * (-dir.x) * pivotRightDist, radiusOfLets));
    //    if (isALet && leftColliders.Count>1)
    //    {
    //        foreach (Collider2D c in rightColliders)
    //        {
    //            Debug.Log(c.name);
    //            if (c.gameObject == Hero.Instance.gameObject && !Hero.Instance.isDead)
    //            {
    //                sprite.flipX = true;
    //                 State = States.attack;
    //                return false;
    //            }
    //        }
    //        return true;
    //    }
    //    return false;
    //}
    //private void Move()
    //{
    //    //Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position /*+ transform.up * 0.1f*/ + transform.right * dir.x * pivotRightDist, radiusOfLets);//возвращает массив коллайдеров, находящийся вокруг указанной точки в указанном радиусе
    //    //if (colliders.Length > 1)
    //    //{
    //    if (colliderOfHero != null)
    //    {
    //        Debug.Log(colliderOfHero.name);
    //        State = States.attack;
    //        colliderOfHero = null;
    //    }
    //    // bool isHero = false;
    //    //foreach (Collider2D c in colliders)
    //    //{
    //    //    //if (c.gameObject == Hero.Instance.gameObject && !Hero.Instance.isDead)
    //    //{
    //    //isHero = true;

    //    // break;
    //    //}
    //    //}
    //    if (Physics2D.OverlapCircleAll(transform.position /*+ transform.up * 0.1f*/ + transform.right * dir.x * pivotRightDist, radiusOfLets).Length>1)//возвращает массив коллайдеров, находящийся вокруг указанной точки в указанном радиусе)
    //            dir.x *= -1f; //меняем направление движения на противоположное          
    //    transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);//перемещение врага
    //    sprite.flipX = dir.x < 0.0f; //поворот монстра(переключение галочки)
    //}
    public override void GetDamage()
    {
        lives--;
        isHit = true;
        StartCoroutine(HitAnimation());
        State = States.hit;
        if (lives < 1)
            Die();
    }
    private IEnumerator HitAnimation()
    {
        yield return new WaitForSeconds(0.233f);
        isHit = false;
    }
    private IEnumerator AttackHero()
    {
        yield return new WaitForSeconds(0.4f);
    }
    private void OnAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRadius);
        foreach (Collider2D collider in colliders)
        {
            if (!Hero.Instance.isHit)
                Hero.Instance.GetDamage();
            return;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "Hero" && !Hero.Instance.isHit && !Hero.Instance.isAttacking && !Hero.Instance.isDead)
        {
            Debug.Log("OnCollision1");
            Hero.Instance.GetDamage();
        }
     }
    private struct ColliderOfHero
    {
        Collider2D collider;
        bool isHero;
        public ColliderOfHero(Collider2D collider/*, bool isHero=true*/)
        {
            this.collider = collider;
            isHero = true;
        }

    }
    private enum States
    {
        idle,
        walk,
        attack,
        hit
    }
}

