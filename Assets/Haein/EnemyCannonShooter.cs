using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannonShooter : Enemy
{
    private float shootDelay = 1f;

    protected override void Awake()
    {
        base.Awake();
    }
    
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
        if (shootDelay > 0)
        {
            shootDelay -= Time.deltaTime;
        }
        else
        {
            Shoot();
            shootDelay = 1f;
        }
    }
    
    private void Shoot()
    {
        GameObject bulletPrefab = Resources.Load<GameObject>("Prefabs/EnemyBullet");
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<EnemyBullet>().SetDamage(5);
    }
}
