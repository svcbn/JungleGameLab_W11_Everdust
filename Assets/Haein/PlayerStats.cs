using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private int maxHp = 100;
    private int curHp;

    void Start()
    {
        curHp = maxHp;
    }
    void Update()
    {
        
    }

    public void Hit(int damage)
    {
        GetComponent<DamageFlash>().CallDamageFlash();
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
