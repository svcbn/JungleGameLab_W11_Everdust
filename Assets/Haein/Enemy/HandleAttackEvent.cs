using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleAttackEvent : MonoBehaviour
{
    public GameObject _hitBox;
    public GameObject _attackBox;
    public GameObject movingLeafSpike;
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
        Debug.Log("CreateMovingProjectile");
        int createNum = 2;
        float distance = 5f;
        if (movingLeafSpike == null) return;
        Transform playerTransform = PlayerManager.Instance.player.transform;
        Vector3 playerPosition = playerTransform.position;

        for (int i = 0; i < createNum; i++)
        {
            // 원형 경로 계산
            float angle = i * (360f / createNum);
            float radians = angle * Mathf.Deg2Rad;

            // X와 Z 위치를 계산하여 새로운 위치를 설정
            float xOffset = Mathf.Cos(radians) * distance;
            float zOffset = Mathf.Sin(radians) * distance;
            Vector3 spawnPosition = playerPosition + new Vector3(xOffset, 0f, zOffset);

            // movingLeafSpike를 생성하고 위치를 설정
            GameObject newProjectile = Instantiate(movingLeafSpike, spawnPosition, Quaternion.identity);

            // 추가 설정이 필요한 경우, 예를 들어 방향 설정 또는 속도 설정
            // newProjectile.GetComponent<MovingLeafSpikeScript>().SetDirectionAndSpeed(...);
        }
    }
}
