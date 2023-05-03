using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : Entity
{
    public bool isAttacking=true;
    public bool isRecharged=false;
    public Transform attackPos; //������� �����
    public float attackRange; //��������� �����
    public LayerMask enemy; //���� � �������
    [SerializeField] private float speed = 3f; //����������, ��������
    [SerializeField] private List<Image> hearts;//���������� ������
    [SerializeField] private float jumpForce = 3f;//���� ������
    private bool isGrounded = false;//����������, ���������� �������� ��������� �� �������� �� �����
    //private int allLives;
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
        isRecharged = true;
        lives = 3;
    }
    private void FixedUpdate()
    {
        if (CheckGround()) State = States.idle;//���� �� �����, ������ �����
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
    private void Run() //����� ��� ����
    {
        if (CheckGround()) State = States.run;
        Vector3 dir = transform.right * Input.GetAxis("Horizontal");
        transform.position = Vector3.MoveTowards(transform.position, transform.position+dir,speed*Time.deltaTime);//����� ��������(���������: ������� ���������, ����� ��� �����������, ��������)
        sprite.flipX = dir.x < 0.0f;//������� ���������(������������ �������)
    }
    private void Attack()
    {
        if (isGrounded && isRecharged)
        {
            State = States.attack;
            isAttacking = true;
            isRecharged = false;
            StartCoroutine(AttackAnimation()); //�������� ����������� ����������� ��������� ������
            StartCoroutine(AttackCoolDown());
        }
    }
    private IEnumerator AttackAnimation() //������� ������� ����� 
    {
        yield return new WaitForSeconds(0.4f); //������ ������������ ����� ��������� ������ � ���������� ������ ��������� �����
        isAttacking = false;
    }
    private IEnumerator AttackCoolDown() //������� ������� �����������
    {
        yield return new WaitForSeconds(0.5f);
        isRecharged = true;
    }
    private void OnAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);//���������� ������ �����������, ����������� ������ ��������� ����� � ��������� �������
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<Entity>().GetDamage();
        }
    }
    private void OnDrawGizmosSelected() //���������� �����, ������������ ������ �����
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
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

public enum States//������ ���������
{
    idle, 
    run,
    jump,
    attack
}
