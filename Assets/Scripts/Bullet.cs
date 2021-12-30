using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage; // 공격력
    public float speed; // 속력
    public bool targeting; // 목표를 따라가는지 여부
    public float life; // 수명

    private Enemy target; // 목표
    private Vector3 direction; // 날아가는 방향

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (targeting)
        {
            direction = (target.transform.position - transform.position).normalized;
        }
        transform.Translate(direction * speed * Time.deltaTime);

        life -= Time.deltaTime;
        if (life < 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetTarget(Enemy e)
    {
        target = e;
        direction = (target.transform.position - transform.position).normalized;
    }

    public int GetDamange()
    {
        return damage;
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Enemy"))
        {
            // todo: 관통력 개념 추가하기
            Destroy(gameObject);
        }
	}
}
