using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject effectObject;

    protected int damage; // 공격력
    protected float speed; // 속력
    protected bool targeting; // 목표를 따라가는지 여부
    protected float life; // 수명
    protected int hp; // 관통력

    public int GetDamage() { return damage; }

    protected Enemy target; // 목표
    protected Vector3 direction; // 날아가는 방향

    protected Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        life -= Time.deltaTime;
        if (life < 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnDestroy()
    {
        if (effectObject != null)
        {
            Instantiate(effectObject, transform.position, transform.rotation);
        }
    }

    // 투사체 이동 (override 가능)
    protected virtual void Move()
    {
        if (targeting && target != null)
        {
            direction = (target.transform.position - rb.position).normalized;
        }
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    public void SetBulletInfo(int d, float s, bool t, float l, int h)
    {
        damage = d;
        speed = s;
        targeting = t;
        life = l;
        hp = h;
    }

    public void SetTarget(Enemy e)
    {
        target = e;
        direction = (target.transform.position - transform.position).normalized;
    }

    public void SetDirection(Vector3 d)
    {
        direction = d;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (hp > 0)
            {
                Enemy enemy = other.GetComponent<Enemy>();
                enemy.SubtractHp(damage);
                hp -= 1;
                if (hp <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
