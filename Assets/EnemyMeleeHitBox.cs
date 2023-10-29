using UnityEngine;

public class EnemyMeleeHitBox : MonoBehaviour
{
    private const float ParryTime = 0.3f;

    private BoxCollider2D _collider;
    private Vector2 _startingOffset;
    private bool _isParried = false;
    private ParryingTest _parry;


    private float Timer { get; set; }
    public int Damage { get; set; }

    public void ActivateHitBox(float delay)
    {
        Timer = delay;
    }

    public void TryGetParried(float angle, GameObject boss = null)
    {
        if (Timer < ParryTime && Timer > 0)
        {
            _isParried = true;
            _parry.TriggerParry(angle, boss);
        }
    }

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _startingOffset = transform.localPosition;
    }

    private void Start()
    {
        _parry = PlayerManager.Instance.player.GetComponentInChildren<ParryingTest>();
    }

    private void Update()
    {
        if (Timer > 0) 
        { 
            Timer -= Time.deltaTime;

            if (Timer <= 0)
            {
                if ( _isParried )
                {
                    _isParried = false;
                    return;
                }
                //Ÿ�� ���� ����.
                Vector2 offset = _startingOffset + _collider.offset;
                Collider2D[] playerCols = Physics2D.OverlapBoxAll((Vector2) transform.position + offset, _collider.size, LayerMask.GetMask("Player"));
                for (int i = 0; i < playerCols.Length; i++)
                {
                    if (playerCols[i].GetComponent<PlayerStats>() is PlayerStats stat)
                    {
                        //Debug.Log("Hit");
                        stat.Hit(Damage);
                    }
                }
            }
        }
        //Debug.Log(_isParried);
    }

}
