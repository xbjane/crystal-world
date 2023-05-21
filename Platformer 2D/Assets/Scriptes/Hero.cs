using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : Entity
{
    [SerializeField] AudioSource audioSource;
    public AudioClip jump;
    public AudioClip damage;
    public AudioClip hit;
    public AudioClip hitEnemy;
    public bool isAttacking;
    public bool isRecharged;
    public bool isHit;
    public bool isDead;
    public Transform attackPos; //позици€ атаки
    public float attackRange; //дальность атаки
    public LayerMask enemy; //слой с врагами
    [SerializeField] private float speed = 3f; //переменные, скорость
    [SerializeField] private List<Image> hearts;//количество жизней
    [SerializeField] private float jumpForce = 3f;//сила прыжка
    private bool isGrounded;//переменна€, содержаща€ значение находитс€ ли персонаж на земле
    //private int allLives;
    private bool isJumping;
    private Rigidbody2D rb; //ссылки на компоненты
    private SpriteRenderer sprite;
    private Animator anim;
    public Joystick joystick;
    public static Hero Instance { get; set; } //сингелтон, позвол€ющий использовать обращение ко все публичным пол€м и методам без создани€ экхемпл€ра класса
    private States State //создаЄм свойство типа State(по названию списка)
    {
        get { return (States)anim.GetInteger("state"); } //получаем значение State из аниматора
        set { anim.SetInteger("state", (int)value); }//мен€ем на новое
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); //получаем компоненты
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();//компонент находитс€ в дочернем элементе (sprite)
        Instance = this;
        isGrounded = CheckGround();
        //if (isGrounded)
        //    isJumping = false;
        isJumping =false;
        isAttacking = false;
        isHit = false;
        isDead = false;
        isRecharged = true;
        lives = 3;
    }
    private void FixedUpdate()
    {
        if (!isDead)
        {
            isGrounded = CheckGround();
            if (isGrounded && !isHit && !isAttacking) State = States.idle;//если на земле, значит стоим
            if (!isAttacking && joystick.Horizontal != 0)
                Run();
            if (!isAttacking && isGrounded && joystick.Vertical >= 0.5)
                Jump();
        }
        //if (lives < allLives)
        //{
        //    allLives--;
        //    Destroy(hearts[lives - 1]);
        //}

    }
    private void Jump()
    {
        if (!isJumping)
        {
            isJumping = true;
            State = States.jump;
            rb.velocity = Vector2.up * jumpForce; //линейна€ скорость по направлению
            audioSource.PlayOneShot(jump);
            StartCoroutine(WaitForJump());
        }
        //rb.AddForce(transform.up*jumpForce, ForceMode2D.Impulse);
    }
    private IEnumerator WaitForJump()
    {
        yield return new WaitForSeconds(0.2f);
        isJumping = false;
    }
    private void Run() //метод дл€ бега
    {
        if (CheckGround()) State = States.run;
        Vector3 dir = transform.right * joystick.Horizontal;
        transform.position = Vector3.MoveTowards(transform.position, transform.position+dir,speed*Time.deltaTime);//задаЄм движение(параметры: текущее положение, место дл€ перемещени€, скорость)
        sprite.flipX = dir.x < 0.0f;//поворот персонажа(переключение галочки)
    }
    public void Attack()
    {
        if (isGrounded && isRecharged)
        {
            State = States.attack;
            isAttacking = true;
            isRecharged = false;
            StartCoroutine(AttackAnimation()); //корутина выполн€етс€ параллельно основному потоку
            StartCoroutine(AttackCoolDown());
        }
    }
    private IEnumerator AttackAnimation() //подсчЄт времени атаки 
    {
        yield return new WaitForSeconds(0.35f); //отдать процессорное врем€ основному потоку и продолжить спуст€ указанное врем€
        isAttacking = false;
    }
    private IEnumerator AttackCoolDown() //подсчЄт времени перезар€дки
    {
        yield return new WaitForSeconds(0.5f);
        isRecharged = true;
    }
    private void OnAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);//возвращает массив коллайдеров, наход€щийс€ вокруг указанной точки в указанном радиусе
        if (colliders.Length == 0)
        {
            audioSource.PlayOneShot(hit);
        }
        else
        {
            audioSource.PlayOneShot(hitEnemy);
            for (int i = 0; i < colliders.Length; i++)
            {
                //colliders[i].GetComponent<Entity>().GetDamage();
               // if (colliders[i].GetComponent<Entity>().lives != 0)
                    DamageToEnemy(colliders[i]);
                //    colliders[i].GetComponent<Entity>().
                //colliders[i].GetComponent<Entity>().
                //StartCoroutine(EnemyOnAttack(colliders[i]));
            }
        }
    }
    private void DamageToEnemy(Collider2D collider)
    {
        if (collider.GetComponent<Entity>() is Enemy/*|| collider.GetComponent<Entity>() is Worm*/)
            collider.GetComponent<Entity>().GetDamage();
    }
    private void OnDrawGizmosSelected() //нарисовать сферу, показывающую радиус атаки
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
    private bool CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position + transform.up * (-0.2f), 0.08f);//создаЄм массив коллайдеров(смещение системы кординат к ногам помогает искать колайдеры у ног)
        isGrounded = collider.Length>1;//(1 - коллайдер персонажа, который тоже считаетс€)     
        if (!isGrounded) State = States.jump;
        return isGrounded;
    }
    public override void Die()
    {
        Debug.Log("Die()");
        State = States.death;
        isDead = true;
        StartCoroutine(Death());      
    }
    private IEnumerator Death()
    {
        yield return new WaitForSeconds(0.767f);
        //Destroy(this.gameObject);
    }
    public override void GetDamage()
    {
        if (!isHit)
        {
            audioSource.PlayOneShot(damage);
            lives--;
            Destroy(hearts[lives].gameObject);
            if (lives == 0)
                Die();
            if (!isDead)
            {
                isHit = true;
                StartCoroutine(HitAnimation());
                //for (int i=0;i<3;i++)
                State = States.hit;
            }
        }
    }
    private IEnumerator HitAnimation()
    {
        yield return new WaitForSeconds(0.767f);
        isHit = false;
    }
    //private IEnumerator EnemyOnAttack(Collider2D enemy) //корутина дл€ эффекта удара по врагу
    //{
    //    SpriteRenderer enemyColor = enemy.GetComponentInChildren<SpriteRenderer>();
    //    enemyColor.color = new Color(0.95f, 0.49f, 0.43f);
    //    yield return new WaitForSeconds(0.2f);
    //    enemyColor.color = new Color(1, 1, 1);
    //}
    private enum States//список состо€ний
    {
        idle,
        run,
        jump,
        attack, 
        hit,
        death,
    }
}

