using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType { normal, spread }

public class Bullet : MonoBehaviour
{
    public BulletType bulletType;

    private int damage; // 공격력
    private float speed; // 속력
    private bool targeting; // 목표를 따라가는지 여부
    private float life; // 수명
    private int hp; // 관통력

    private Enemy target; // 목표
    private Vector3 direction; // 날아가는 방향

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (bulletType)
        {
            case BulletType.normal:
                if (targeting)
                {
                    direction = (target.transform.position - transform.position).normalized;
                }
                transform.Translate(direction * speed * Time.deltaTime);
                break;
            case BulletType.spread:
                transform.localScale += speed * Time.deltaTime * new Vector3(1, 0, 1);
                break;
        }

        life -= Time.deltaTime;
        if (life < 0)
        {
            Destroy(gameObject);
        }
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
