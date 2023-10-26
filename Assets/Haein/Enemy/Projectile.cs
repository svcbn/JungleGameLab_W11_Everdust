using UnityEngine;
using UnityEngine.Networking.Types;

public abstract class Projectile : MonoBehaviour
{
    private int maxHp;
    public int MaxHp{
        get { return maxHp; }
        set 
        { 
            maxHp = value;
            if(value < _curHp)
                _curHp = value;
        }
    }

    protected int _curHp;
    protected float _hitDelay = 0f;

    protected virtual void Awake()
    {
        MaxHp = 1;
        _curHp = MaxHp;
    }
    
    protected virtual void Start(){}
    protected virtual void Update()
    {
        if (_hitDelay > 0f)
        {
            _hitDelay -= Time.deltaTime;
        }
    }
    
    public void TakeHit(bool hitWeakness = false)
    {
        if (TryGetComponent(out HandleWeaknessCircle weaknessCircle))
        {
            if (!weaknessCircle.IsWeaknessAttacked())
            {
                hitWeakness = false;
            }
        }
        int dmg = hitWeakness ? WeaponStats.damage * WeaponStats.criticalMultiplier : WeaponStats.damage;
        Hit(dmg, hitWeakness);
    }

    private void Hit(int _damage, bool _hitWeakness)
    {
        _hitDelay = 0.3f;
        _curHp -= _damage;

        if (_curHp <= 0)
        {
            Destroy(gameObject);
        }
    }

}