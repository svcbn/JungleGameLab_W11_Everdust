using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleAttackEvent : MonoBehaviour
{
    public GameObject _hitBox;
    public GameObject _attackBox;
    public GameObject movingLeafSpike;
    private int createNum = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateHitBox()
    {
        int dir = GetComponent<SpriteRenderer>().flipX ? 1 : -1;
        Instantiate(_hitBox, transform.position + new Vector3(dir * 2.3f, -.8f, 0f), Quaternion.identity);
    }

    public void createAttackBox()
    {
        int dir = GetComponent<SpriteRenderer>().flipX ? 1 : -1;
        Instantiate(_attackBox, transform.position + new Vector3(dir * 2.3f, -.8f, 0f), Quaternion.identity);
    }
    
    public void CreateMovingProjectile()
    {
        if (movingLeafSpike == null) return;
        createNum = 2;
        InvokeRepeating("CreateProjectile", 0f, 0.5f);

    }

    private void CreateProjectile()
    {
        float distance = 5f;
        if (createNum <= 0)
        {
            CancelInvoke("CreateProjectile");
            return;
        }
        
        float angle = (createNum - 1) * (360f / 2); // 2로 나누어 180도 회전
        float radians = angle * Mathf.Deg2Rad;
        
        float xOffset = Mathf.Cos(radians) * distance;
        float zOffset = Mathf.Sin(radians) * distance;
        Transform playerTransform = PlayerManager.Instance.player.transform;
        Vector3 playerPosition = playerTransform.position;
        Vector3 spawnPosition = playerPosition + new Vector3(xOffset, 0f, zOffset);
        
        GameObject newProjectile = Instantiate(movingLeafSpike, spawnPosition, Quaternion.identity);
        createNum--;
    }

}
