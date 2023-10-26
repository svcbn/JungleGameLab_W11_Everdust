using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HandleMovingLeafSpike : Projectile
{
    private bool isMoveToPlayer = false;
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = PlayerManager.Instance.player.transform;
        HandleLeafSpike();
    }

    private void Update()
    {
        base.Update();
        if (playerTransform != null)
        {
            Vector2 dir = new Vector2(transform.position.x - playerTransform.position.x, transform.position.y - playerTransform.position.y);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion angleAxis = Quaternion.AngleAxis(angle + 90f, Vector3.forward);
            Quaternion rotation = Quaternion.Slerp(transform.rotation, angleAxis, 10f * Time.deltaTime);
            transform.rotation = rotation;

            if (isMoveToPlayer)
            {
                transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, 15f * Time.deltaTime);
            }
        }
    }

    private void HandleLeafSpike()
    {
        if (playerTransform != null)
        {
            transform.DOMove(transform.position - (playerTransform.position - transform.position), 0.5f)
                .SetEase(Ease.Linear) 
                .OnComplete(() =>
                {
                    isMoveToPlayer = true;
                });
        }
    }
    
    private void HandleCollision()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
        if (other.CompareTag("LeafSpike"))
        {
            HandleCollision();
        }
        
        if (other.CompareTag("Player"))
        {
            //플레이어 데미지 스크립트 추가
            HandleCollision();
        }
    }
}