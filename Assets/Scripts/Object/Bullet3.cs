using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet3 : Bullet
{
    private EnemyManager enemyManager;
    public float bombRange;

    protected override void OnStart()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (hp > 0)
            {
                // 가까운 순으로 hp 개수만큼의 적에게 데미지
                SortedDictionary<float, Enemy> distanceDict = new SortedDictionary<float, Enemy>();
                foreach (Enemy e in enemyManager.GetEnemySet())
                {
                    Collider collider = e.GetComponent<Collider>();
                    Vector3 closestPoint = collider.ClosestPoint(transform.position);
                    float distance = Vector3.Distance(transform.position, closestPoint);
                    if (distance <= bombRange) distanceDict.Add(distance, e);
                }
                foreach (Enemy e in distanceDict.Values)
                {
                    e.SubtractHp(damage);
                    hp -= 1;
                    if (hp <= 0) break;
                }
                Destroy(gameObject);
            }
        }
    }
}
