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
    public Transform attackPos; //������� �����
    public float attackRange; //��������� �����
    public LayerMask enemy; //���� � �������
    [SerializeField] private float speed = 3f; //����������, ��������
    [SerializeField] private List<Image> hearts;//���������� ������
    [SerializeField] private float jumpForce = 3f;//���� ������
    private bool isGrounded;//����������, ���������� �������� ��������� �� �������� �� �����
    //private int allLives;
    private bool isJumping;
    private Rigidbody2D rb; //������ �� ����������
    private SpriteRenderer sprite;
    private Animator anim;
    public Joystick joystick;
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
            if (isGrounded && !isHit && !isAttacking) State = States.idle;//���� �� �����, ������ �����
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
            rb.velocity = Vector2.up * jumpForce; //�������� �������� �� �����������
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
    private void Run() //����� ��� ����
    {
        if (CheckGround()) State = States.run;
        Vector3 dir = transform.right * joystick.Horizontal;
        transform.position = Vector3.MoveTowards(transform.position, transform.position+dir,speed*Time.deltaTime);//����� ��������(���������: ������� ���������, ����� ��� �����������, ��������)
        sprite.flipX = dir.x < 0.0f;//������� ���������(������������ �������)
    }
    public void Attack()
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
        yield return new WaitForSeconds(0.35f); //������ ������������ ����� ��������� ������ � ���������� ������ ��������� �����
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
    private void OnDrawGizmosSelected() //���������� �����, ������������ ������ �����
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
    private bool CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position + transform.up * (-0.2f), 0.08f);//������ ������ �����������(�������� ������� �������� � ����� �������� ������ ��������� � ���)
        isGrounded = collider.Length>1;//(1 - ��������� ���������, ������� ���� ���������)     
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
    //private IEnumerator EnemyOnAttack(Collider2D enemy) //�������� ��� ������� ����� �� �����
    //{
    //    SpriteRenderer enemyColor = enemy.GetComponentInChildren<SpriteRenderer>();
    //    enemyColor.color = new Color(0.95f, 0.49f, 0.43f);
    //    yield return new WaitForSeconds(0.2f);
    //    enemyColor.color = new Color(1, 1, 1);
    //}
    private enum States//������ ���������
    {
        idle,
        run,
        jump,
        attack, 
        hit,
        death,
    }
}

