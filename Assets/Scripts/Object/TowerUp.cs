using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUp : Tower
{
    private Bullet bullet;

    // Update is called once per frame
    void Update()
    {
        // 묻지도 따지지도 않고 일단 투사체 발사
        if (attackable)
        {
            LineRenderer pathManager = FindObjectOfType<LineRenderer>();
            Vector3[] path = new Vector3[pathManager.positionCount];
            pathManager.GetPositions(path);

            bullet = Instantiate(GetBullet(), transform.position, transform.rotation, transform).GetComponent<Bullet>();
            bullet.SetBulletInfo(GetDamage(), GetSpeed(), GetTargeting(), GetLife(), GetBulletHp());

            // 투사체의 y 높이 결정: 수직으로 닿는 path의 y축의 최댓값
            float maxY = transform.position.y;
            for (int i = 0; i < path.Length - 1; i++)
            {
                Vector2 v1 = new Vector2(transform.position.x - path[i].x, transform.position.z - path[i].z);
                Vector2 v2 = new Vector2(transform.position.x - path[i + 1].x, transform.position.z - path[i + 1].z);
                if (((path[i].x == transform.position.x && path[i].z == transform.position.z) ||
                    (path[i + 1].x == transform.position.x && path[i + 1].z == transform.position.z)) ||
                    Vector2.Angle(v1, v2) == 180)
                {
                    if (maxY < path[i].y) maxY = path[i].y;
                    if (maxY < path[i + 1].y) maxY = path[i + 1].y;
                }
            }
            bullet.transform.localScale = new Vector3(1, maxY - transform.position.y + 0.4f, 1);

            StartCoroutine(SetAttackable());
            attackable = false;
        }

        if (bullet != null)
        {
            bullet.SetBulletInfo(GetDamage(), GetSpeed(), GetTargeting(), GetLife(), GetBulletHp());
        }
    }
}
