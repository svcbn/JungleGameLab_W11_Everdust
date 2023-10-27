using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Myd.Platform;

public class PlayerStats : MonoBehaviour
{
    private int maxHp = 100;
    private int curHp;
    PlayerController playerController;

    void Start()
    {
        curHp = maxHp;
        playerController = Game.Instance.player.GetPlayerController();
    }
    void Update()
    {

    }

    public void Hit(int damage)
    {
        if (!playerController.CanHit) return;

        GetComponent<DamageFlash>().CallDamageFlash();
        
        //데미지 텍스트
        float xOffset = Random.Range(-0.5f, 0.5f);
        float yOffset = Random.Range(0f, 3f);

        Vector3 positionWithRandomOffset = transform.position + new Vector3(xOffset, yOffset, 0f);
        GameObject damageTextPrefab = Resources.Load<GameObject>("Prefabs/UI/DamageText");
        GameObject damageText = Instantiate(damageTextPrefab, positionWithRandomOffset, Quaternion.identity);
        damageText.GetComponent<MoveAndDestroy>()._text = "-" + damage;
        
        curHp -= damage;
        if (curHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died");
    }
}
