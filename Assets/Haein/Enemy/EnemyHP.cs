using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public int maxHp = 100;
    public GameObject weaknessCircle;

    private int _curHp;
    private float _hitDelay = 0f;

    private HandleWeaknessCircle handleWeaknessCircle;

    private void Awake()
    {
        handleWeaknessCircle = GetComponent<HandleWeaknessCircle>();

        _curHp = maxHp;
    }

    private void Update()
    {
        if (_hitDelay > 0f)
        {
            _hitDelay -= Time.deltaTime;
        }
    }
    
    /// <summary>
    /// 적이 데미지를 입는다. 플레이어 쪽에서 호출할 것.
    /// </summary>
    /// <param name="hitWeakness"></param>
    public void TakeHit(bool hitWeakness = false)
    {
        int dmg = hitWeakness ? WeaponStats.damage * WeaponStats.criticalMultiplier : WeaponStats.damage;

        Hit(dmg);
    }

    private void Hit(int _damage)
    {
        _hitDelay = 0.3f;
        _curHp -= _damage;
        print($"{name} HP: {_curHp} (-{_damage})");
        if (_curHp <= 0)
        {
            Destroy(gameObject);
        }
    }
}