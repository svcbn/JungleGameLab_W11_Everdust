using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannonShooter : Enemy
{
    private float shootDelay = 2f;

    protected override void Awake()
    {
        base.Awake();
        MaxHp = 30;
        _curHp = MaxHp;
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
        if (transform.position.x > 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position + new Vector3(-2f, .5f, 0f), Quaternion.identity);
            bullet.GetComponent<EnemyBullet>().moveDir = -1;
        }
        else
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position + new Vector3(2f, .5f, 0f), Quaternion.identity);
            bullet.GetComponent<EnemyBullet>().moveDir = 1;
        }
    }
}
