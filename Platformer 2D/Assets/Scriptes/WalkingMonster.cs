using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingMonster : Entity
{
    [SerializeField] private float speed = 3.5f;
    private Animator anim;
    private Vector3 dir;
    private SpriteRenderer sprite;
    private StatesOfMonster State
    {
        get { return (StatesOfMonster)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }
    }
    private void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        dir = transform.right;
        lives = 2;
    }
    private void Update()
    {
        if (CheckLets())
            State = StatesOfMonster.idle;
        else
        {
            State = StatesOfMonster.walk;
            Move();
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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position /*+ transform.up * 0.1f*/ + transform.right * dir.x * 0.1f, 0.2f);//���������� ������ �����������, ����������� ������ ��������� ����� � ��������� �������
        if (colliders.Length > 1)
        {
            Debug.Log("Change dir");
            dir.x *= -1f; //������ ����������� �������� �� ���������������          
        }
        Debug.Log(dir.x);
        transform.position = Vector3.MoveTowards(transform.position, transform.position+dir, speed*Time.deltaTime);//����������� �����
        sprite.flipX = dir.x < 0.0f; //������� �������(������������ �������)
    }
    private void OnCollisionEnter2D(Collision2D collision) //����������, ����� ��������� ������� ������� �������� � ����������� ����� �������
    {
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            Hero.Instance.GetDamage();
        }
    }
}
public enum StatesOfMonster
{
    idle, 
    walk
}
