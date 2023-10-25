using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private int _criticalMultiplier = 4;

    public static int damage;
    public static int criticalMultiplier;

    private void Awake()
    {
        damage = _damage;
        criticalMultiplier = _criticalMultiplier;
    }
}
