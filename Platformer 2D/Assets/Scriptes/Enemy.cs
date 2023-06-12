using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public float attackRadius;
    public float pivotRightDist;
    public float radiusOfLets;
    [SerializeField] private float speed;
    private Vector3 dir;
    private SpriteRenderer sprite;
    private Animator anim;
    private bool isHit;
    private bool isDead;
    public GameObject crystal;
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
        isDead = false;
    }
    private void Start()
    {
        dir = transform.right;
        lives = 3;
    }
    private void FixedUpdate()
    {
        if (!isHit&&!isDead)
        {
            Behavior();
        }
    }
    private void Behavior()
    {
        if (Hero.Instance.isDead)
        {
            State = States.idle;
            return;
        }
        else
        {
            Collider2D[] rightColliders = Physics2D.OverlapCircleAll(transform.position/* + transform.up * 0.5f*/ + transform.right * dir.x * pivotRightDist, radiusOfLets);
            if (rightColliders.Length > 1 && !(rightColliders.Length == 2 && rightColliders[0].gameObject.tag == "crystal"))
            {
                bool isHero = false;
                CheckLets(rightColliders, ref isHero);
                if (!isHero)
                {
                    dir.x *= -1f;
                    sprite.flipX = dir.x < 0.0f;
                    Collider2D[] leftColliders = Physics2D.OverlapCircleAll(transform.position/* + transform.up * 0.5f*/ + transform.right * dir.x * pivotRightDist, radiusOfLets);
                    if (leftColliders.Length > 1 && !(leftColliders.Length == 2 && leftColliders[0].gameObject.tag == "crystal"))
                    {
                        CheckLets(leftColliders, ref isHero);
                        if (!isHero)
                            State = States.idle;
                        else return;
                    }
                }
                else return;
            }
            State = States.walk;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);//перемещение врага

        }
    }
    private void CheckLets(Collider2D[] colliders, ref bool isHero)
    {
        foreach (Collider2D c in colliders)
        {
            if (c.gameObject == Hero.Instance.gameObject && !Hero.Instance.isDead)
            {
                isHero = true;
                State = States.attack;
                return;
            }
        }
    }
    public override void GetDamage()
    {
        lives--;
        isHit = true;
        StartCoroutine(HitAnimation());
        State = States.hit;
        if (lives < 1)
        {
            State = States.destroy;
            isDead = true;
        }
    }
    private void OnDestroyEnemy()
    {
        Vector2 posOfReward = transform.position;
        Die();
        Instantiate(crystal, posOfReward, Quaternion.identity);
    }
    private IEnumerator HitAnimation()
    {
        yield return new WaitForSeconds(0.233f);
        isHit = false;
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
        if (collision.gameObject.name == "Hero" && !Hero.Instance.isHit && !Hero.Instance.isAttacking && !Hero.Instance.isDead)
        {
            Hero.Instance.GetDamage();
        }
     }
    private enum States
    {
        idle,
        walk,
        attack,
        hit, 
        destroy
    }
}

