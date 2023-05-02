using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Entity
{
    [SerializeField] private float speed = 3f; //переменные, скорость
    [SerializeField] private int lives = 5;//количество жизней
    [SerializeField] private float jumpForce = 3f;//сила прыжка
    private bool isGrounded = false;//переменная, содержащая значение находится ли персонаж на земле

    private Rigidbody2D rb; //ссылки на компоненты
    private Animator anim;
    private SpriteRenderer sprite;

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
    }
    private void FixedUpdate()
    {
        if (CheckGround()) State = States.idle;//если на земле, значит стоим
        if (Input.GetButton("Horizontal"))
            Run();
        if (CheckGround() && Input.GetButton("Jump"))
        {
            Jump();
        }
    }
    private void Jump()
    {
        State = States.jump;
        rb.AddForce(transform.up*jumpForce, ForceMode2D.Impulse);
    }
    private void Run() //метод для бега
    {
        if (CheckGround()) State = States.run;
        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position+dir,speed*Time.deltaTime);//задаём движение(параметры: текущее положение, место для перемещения, скорость)
        sprite.flipX = dir.x < 0.0f;//поворот персонажа(переключение галочки)
    }
    private bool CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position + transform.up * (-0.1f), 0.2f);//создаём массив коллайдеров(смещение системы кординат к ногам помогает искать колайдеры у ног)
        isGrounded = collider.Length>1;//(1 - коллайдер персонажа, который тоже считается)
        if (!isGrounded) State = States.jump;
        return isGrounded;
    }
    public override void GetDamage()
    {
        lives -= 1;
        Debug.Log(lives);
        if (lives < 1)
            Die();
    }
}

public enum States//список состояний
{
    idle, 
    run,
    jump
}
