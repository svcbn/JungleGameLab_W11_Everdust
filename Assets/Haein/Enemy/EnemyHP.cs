using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    public int maxHp = 100;
    private int curHp;
    private float hitDelay = 0f;
    public GameObject weaknessCircle;

    private void Start()
    {
        curHp = maxHp;
    }

    private void Update()
    {
        if (hitDelay > 0f)
        {
            hitDelay -= Time.deltaTime;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.GetMask("Weapon"))
        {
            if (PlayerManager.Instance.player != null && hitDelay <= 0f)
            {
                if (PlayerManager.Instance.player.GetComponent<HandleWeaponClick>().isPoke)
                {
                    if (GetComponent<HandleWeaknessCircle>().IsWeaknessAttacked() && other.IsTouching(weaknessCircle.GetComponent<Collider2D>()))
                    {
                        Hit(other.GetComponent<WeaponStats>().damage * other.GetComponent<WeaponStats>().criticalMultiplier);
                        return;
                    }
                    Hit(other.GetComponent<WeaponStats>().damage);
                }
            }
        }
    }

    private void Hit(int _damage)
    {
        hitDelay = 0.3f;
        curHp -= _damage;
        if (curHp <= 0)
        {
            Destroy(gameObject);
        }
    }
}