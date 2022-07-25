using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet2 : Bullet
{
    protected override void Move()
    {
        transform.localScale += speed * Time.deltaTime * new Vector3(1, 0, 1);
    }
}
