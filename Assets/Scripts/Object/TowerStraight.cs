using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerStraight : Tower
{
    public Vector3 direction;

    // Update is called once per frame
    void Update()
    {
        if (attackable)
        {
            RaycastHit hit;
            int layerMask = 1 << LayerMask.NameToLayer("Enemy");
            if (Physics.Raycast(transform.position, direction, out hit, Mathf.Infinity, layerMask))
            {
                Bullet newBullet = Instantiate(GetBullet(), transform.position, transform.rotation).GetComponent<Bullet>();
                newBullet.SetBulletInfo(GetDamage(), GetSpeed(), GetTargeting(), GetLife(), GetBulletHp());
                newBullet.SetDirection(direction);
                Invoke("SetAttackable", GetDelay());
                attackable = false;
            }
        }
    }

    public void Rotate()
    {
        direction = Quaternion.AngleAxis(90, Vector3.up) * direction;
        transform.LookAt(transform.position + direction);
    }
}
