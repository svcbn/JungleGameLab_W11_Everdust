using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleBossHitbox : MonoBehaviour
{
    private void OnDestroy()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && PlayerManager.Instance.player != null)
        {
            Debug.Log("Player hit");
            Bounds spriteBounds = spriteRenderer.bounds;
            Transform playerTransform = PlayerManager.Instance.player.transform;
            Vector3 playerPosition = playerTransform.position;
            if (spriteBounds.Contains(playerPosition))
            {
                PlayerManager.Instance.player.GetComponent<PlayerStats>().Hit(10);
            }
        }
    }
}
