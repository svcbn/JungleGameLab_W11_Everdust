using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Projectile
{
    public int moveDir = 1;

    protected override void Update()
    {
        base.Update();
        transform.Translate(moveDir * 30f * Time.deltaTime, 0f, 0f);
    }
}
