using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerAttackbox : MonoBehaviour
{
    private float delay = 1f;
    private bool canDamage = true;
    
    private void Update()
    {
        if (delay > 0)
        {
            delay -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!canDamage) return;
            if (delay < .6f) return;
            //플레이어 데미지 스크립트 추가
            PlayerManager.Instance.player.GetComponent<PlayerStats>().TakeDamage(10);
            canDamage = false;
        }
    }
}
