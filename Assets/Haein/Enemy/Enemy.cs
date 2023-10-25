using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int maxHp = 100;
    public GameObject weaknessCircle;

    protected int _curHp;
    protected float _hitDelay = 0f;

    protected HandleWeaknessCircle _handleWeaknessCircle;

    protected virtual void Awake()
    {
        _handleWeaknessCircle = GetComponent<HandleWeaknessCircle>();

        _curHp = maxHp;
    }

    protected virtual void Update()
    {
        if (_hitDelay > 0f)
        {
            _hitDelay -= Time.deltaTime;
        }
    }
    
    /// <summary>
    ///�� ���� �������� �Դ´�. �÷��̾� �ʿ��� ȣ���� ��.
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