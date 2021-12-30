using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 타워 오브젝트입니다.
// mouseCursor에서 타워 설치하기 전에는 비활성화 상태입니다.
// 타워를 설치하는 순간 활성화됩니다.
public class Tower : MonoBehaviour
{
    public float radius; // 공격 반경
    public float delay; // 공격 딜레이 시간
    public GameObject bullet; // 투사체

    private Component radiusSphere; // 반경을 나타내는 투명한 구
    private MouseCursor mouseCursor; // 마우스 커서 오브젝트
    private EnemyManager enemyManager;
    private bool select; // 오브젝트가 선택되었는지 나타냄
    private bool attackable; // 공격 딜레이가 지나서 공격 가능한가

    // Start is called before the first frame update
    void Start()
    {
        mouseCursor = FindObjectOfType<MouseCursor>();
        enemyManager = FindObjectOfType<EnemyManager>();
        radiusSphere = transform.GetChild(1);
        radiusSphere.transform.localScale = new Vector3(radius, radius, radius) * 2;
        attackable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackable)
        {
            // 일단은 가까운 적을 목표로 설정
            HashSet<Enemy> set = enemyManager.GetEnemySet();
            Enemy target = null;
            float targetDistance = Mathf.Infinity;
            if (set.Count > 0)
            {
                foreach (Enemy e in set)
                {
                    float distance = (e.transform.position - transform.position).magnitude;
                    if (distance <= radius && distance < targetDistance)
                    {
                        target = e;
                        targetDistance = distance;
                    }
                }
            }

            if (target != null)
            {
                Bullet newBullet = Instantiate(bullet, transform.position, transform.rotation).GetComponent<Bullet>();
                newBullet.SetTarget(target);
                Invoke("SetAttackable", delay);
                attackable = false;
            }
        }
    }

    void SetAttackable()
    {
        attackable = true;
    }

    public void SetSelect(bool b)
    {
        radiusSphere.gameObject.SetActive(b);
        select = b;
    }

    private void OnMouseUpAsButton()
    {
        mouseCursor.OnClickTower(gameObject);
    }
}
