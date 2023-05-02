using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Entity
{
    [SerializeField] private float speed = 3f; //����������, ��������
    [SerializeField] private int lives = 5;//���������� ������
    [SerializeField] private float jumpForce = 3f;//���� ������
    private bool isGrounded = false;//����������, ���������� �������� ��������� �� �������� �� �����

    private Rigidbody2D rb; //������ �� ����������
    private Animator anim;
    private SpriteRenderer sprite;

    public static Hero Instance { get; set; } //���������, ����������� ������������ ��������� �� ��� ��������� ����� � ������� ��� �������� ���������� ������
    private States State //������ �������� ���� State(�� �������� ������)
    {
        get { return (States)anim.GetInteger("state"); } //�������� �������� State �� ���������
        set { anim.SetInteger("state", (int)value); }//������ �� �����
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); //�������� ����������
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();//��������� ��������� � �������� �������� (sprite)
        Instance = this;
    }
    private void FixedUpdate()
    {
        if (CheckGround()) State = States.idle;//���� �� �����, ������ �����
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
    private void Run() //����� ��� ����
    {
        if (CheckGround()) State = States.run;
        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position+dir,speed*Time.deltaTime);//����� ��������(���������: ������� ���������, ����� ��� �����������, ��������)
        sprite.flipX = dir.x < 0.0f;//������� ���������(������������ �������)
    }
    private bool CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position + transform.up * (-0.1f), 0.2f);//������ ������ �����������(�������� ������� �������� � ����� �������� ������ ��������� � ���)
        isGrounded = collider.Length>1;//(1 - ��������� ���������, ������� ���� ���������)
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

public enum States//������ ���������
{
    idle, 
    run,
    jump
}
