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
    
    private DamageFlash _damageFlash;

    protected int _curHp;
    protected float _hitDelay = 0f;

    protected virtual void Awake()
    {
        
        _damageFlash = GetComponent<DamageFlash>();

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
        
        if (Vector3.Magnitude(transform.position) > 200f) // 룸 밖으로 나갔을 시 제거
        {
            EraseProjectile();
        }
    }
    
    public virtual void TakeHit(bool hitWeakness = false, float attackAngle = 0f)
    {
        int dmg = hitWeakness ? WeaponStats.damage * WeaponStats.criticalMultiplier : WeaponStats.damage;
        Hit(dmg, hitWeakness);
    }

    private void Hit(int _damage, bool _hitWeakness)
    {
        string damageStr = _curHp.ToString();
        _hitDelay = 0.3f;
        _curHp -= _damage;
        if(_curHp > 0)
            damageStr = _damage.ToString();
        
        //흰색으로 번쩍이는 쉐이더
        _damageFlash.CallDamageFlash();
        
        //데미지 텍스트
        float xOffset = Random.Range(-0.5f, 0.5f);
        float yOffset = Random.Range(0f, 3f);

        Vector3 positionWithRandomOffset = transform.position + new Vector3(xOffset, yOffset, 0f);

        GameObject damageTextPrefab = Resources.Load<GameObject>("Prefabs/UI/DamageText");
        GameObject damageText = Instantiate(damageTextPrefab, positionWithRandomOffset, Quaternion.identity);
        damageText.GetComponent<MoveAndDestroy>()._text = "-" + damageStr;
        if (!_hitWeakness) damageText.GetComponent<TextMesh>().color = Color.white;
        
        print($"{name} HP: {_curHp} (-{_damage})");
        if (_curHp <= 0)
        {
            EraseProjectile();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //플레이어 데미지 스크립트 추가
            PlayerManager.Instance.player.GetComponent<PlayerStats>().Hit(10);
            HandleCollision();
        }
    }
    
    private void HandleCollision()
    {
        EraseProjectile();
    }
    

    public virtual void EraseProjectile()
    {
        Destroy(gameObject);
    }
}