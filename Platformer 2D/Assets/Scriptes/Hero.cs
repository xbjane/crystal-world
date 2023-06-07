using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hero : Entity
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] TMP_Text youDiedText;
    public AudioClip jump;
    public AudioClip damage;
    public AudioClip hit;
    public AudioClip hitEnemy;
    public bool isAttacking;
    public bool isRecharged;
    public bool isHit;
    public bool isDead;
    public Transform attackPos; //позиция атаки
    public float attackRange; //дальность атаки
    public LayerMask enemy; //слой с врагами
    [SerializeField] private float speed = 3f; //переменные, скорость
    [SerializeField] private List<Image> hearts;//количество жизней
    [SerializeField] private float jumpForce = 3f;//сила прыжка
    private bool isGrounded;//переменная, содержащая значение находится ли персонаж на земле
    //private int allLives;
    private bool isJumping;
    private Rigidbody2D rb; //ссылки на компоненты
    private SpriteRenderer sprite;
    private Animator anim;
    public Joystick joystick;
    public static Hero Instance { get; set; } //сингелтон, позволяющий использовать обращение ко все публичным полям и методам без создания экхемпляра класса
    private States State //создаём свойство типа State(по названию списка)
    {
        get { return (States)anim.GetInteger("state"); } //получаем значение State из аниматора
        set { anim.SetInteger("state", (int)value); }//меняем на новое
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); //получаем компоненты
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();//компонент находится в дочернем элементе (sprite)
        Instance = this;
        isGrounded = CheckGround();
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
        if (transform.position.y < -7f)
            Die();
    }
    private void Jump()
    {
        if (!isJumping)
        {
            isJumping = true;
            Handheld.Vibrate();
            State = States.jump;
            rb.velocity = Vector2.up * jumpForce; //линейная скорость по направлению
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
    private void Run() //метод для бега
    {
        if (CheckGround()) State = States.run;
        Vector3 dir = transform.right * joystick.Horizontal;
        transform.position = Vector3.MoveTowards(transform.position, transform.position+dir,speed*Time.deltaTime);//задаём движение(параметры: текущее положение, место для перемещения, скорость)
        sprite.flipX = dir.x < 0.0f;//поворот персонажа(переключение галочки)
    }
    public void Attack()
    {
        if (isGrounded && isRecharged&&!isDead)
        {
            State = States.attack;
            isAttacking = true;
            isRecharged = false;
            StartCoroutine(AttackAnimation()); //корутина выполняется параллельно основному потоку
            StartCoroutine(AttackCoolDown());
        }
    }
    private IEnumerator AttackAnimation() //подсчёт времени атаки 
    {
        yield return new WaitForSeconds(0.35f); //отдать процессорное время основному потоку и продолжить спустя указанное время
        isAttacking = false;
    }
    private IEnumerator AttackCoolDown() //подсчёт времени перезарядки
    {
        yield return new WaitForSeconds(0.5f);
        isRecharged = true;
    }
    private void OnAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);//возвращает массив коллайдеров, находящийся вокруг указанной точки в указанном радиусе
        if (colliders.Length == 0)
        {
            audioSource.PlayOneShot(hit);
        }
        else
        {
            audioSource.PlayOneShot(hitEnemy);
            for (int i = 0; i < colliders.Length; i++)
            {
                    DamageToEnemy(colliders[i]);
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
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position + transform.up * (-0.2f), 0.08f);//создаём массив коллайдеров(смещение системы кординат к ногам помогает искать колайдеры у ног)
        isGrounded = collider.Length>1;//(1 - коллайдер персонажа, который тоже считается)     
        if (!isGrounded) State = States.jump;
        return isGrounded;
    }
    public override void Die()
    {
        State = States.death;
        isDead = true;
        SaveScore.score = ScoreCount.Instance.score;
        Debug.Log(ScoreCount.Instance.score);
        SaveScore.Instance.CheckScore();
        StartCoroutine(YouDied());
        StartCoroutine(Death());
    }
    private IEnumerator Death()
    {
        yield return new WaitForSeconds(0.900f);
      
    }
    private IEnumerator YouDied()
    {
        int i = 0;
        while (i < 3)
        {
            Debug.Log("YouDied!");
            Debug.Log(i);
            yield return new WaitForSeconds(0.300f);
            youDiedText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.300f);
            youDiedText.gameObject.SetActive(false);
            i++;
        }
      //  youDiedText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.300f);
        SceneLoader.Load(SceneLoader.Scenes.menu);
    }
    public override void GetDamage()
    {
        if (!isHit)
        {
            audioSource.PlayOneShot(damage);
            lives--;
            Destroy(hearts[lives].gameObject);//!!!!!!!!!!!!!!!!!!
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
    private enum States//список состояний
    {
        idle,
        run,
        jump,
        attack, 
        hit,
        death,
    }
}

