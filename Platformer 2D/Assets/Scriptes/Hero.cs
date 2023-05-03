using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : Entity
{
    public bool isAttacking=true;
    public bool isRecharged=false;
    public Transform attackPos; //позици€ атаки
    public float attackRange; //дальность атаки
    public LayerMask enemy; //слой с врагами
    [SerializeField] private float speed = 3f; //переменные, скорость
    [SerializeField] private List<Image> hearts;//количество жизней
    [SerializeField] private float jumpForce = 3f;//сила прыжка
    private bool isGrounded = false;//переменна€, содержаща€ значение находитс€ ли персонаж на земле
    //private int allLives;
    private Rigidbody2D rb; //ссылки на компоненты
    private Animator anim;
    private SpriteRenderer sprite;

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
        isRecharged = true;
        lives = 3;
    }
    private void FixedUpdate()
    {
        if (CheckGround()) State = States.idle;//если на земле, значит стоим
        if (Input.GetButton("Horizontal"))
            Run();
        if (CheckGround() && Input.GetButton("Jump"))
            Jump();
        if (Input.GetButtonDown("Fire1"))
                Attack();
        //if (lives < allLives)
        //{
        //    allLives--;
        //    Destroy(hearts[lives - 1]);
        //}

    }
    private void Jump()
    {
        State = States.jump;
        rb.AddForce(transform.up*jumpForce, ForceMode2D.Impulse);
    }
    private void Run() //метод дл€ бега
    {
        if (CheckGround()) State = States.run;
        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position+dir,speed*Time.deltaTime);//задаЄм движение(параметры: текущее положение, место дл€ перемещени€, скорость)
        sprite.flipX = dir.x < 0.0f;//поворот персонажа(переключение галочки)
    }
    private void Attack()
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
        yield return new WaitForSeconds(0.4f); //отдать процессорное врем€ основному потоку и продолжить спуст€ указанное врем€
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
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<Entity>().GetDamage();
        }
    }
    private void OnDrawGizmosSelected() //нарисовать сферу, показывающую радиус атаки
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
    private bool CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position + transform.up * (-0.1f), 0.2f);//создаЄм массив коллайдеров(смещение системы кординат к ногам помогает искать колайдеры у ног)
        isGrounded = collider.Length>1;//(1 - коллайдер персонажа, который тоже считаетс€)
        if (!isGrounded) State = States.jump;
        return isGrounded;
    }
    public override void GetDamage()
    {
        Debug.Log(lives);
        lives--;
        if (lives >= 1)
            Destroy(hearts[lives].gameObject);
        else
        {
            Destroy(hearts[0].gameObject);
            Die();
        }
    }
}

public enum States//список состо€ний
{
    idle, 
    run,
    jump,
    attack
}
