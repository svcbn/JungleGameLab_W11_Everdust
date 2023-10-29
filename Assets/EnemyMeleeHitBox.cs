using Myd.Platform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeHitBox : MonoBehaviour
{
    public static float parryTime = 0.3f;

    private float _timer;
    private BoxCollider2D _collider;
    private Vector2 _startingOffset;
    private bool _isParried = false;
    private ParryingTest _parry;


    public float Timer
    {
        get => _timer;
        set
        {
            _timer = value;
        }
    }
    public int Damage { get; set; }

    public void ActivateHitBox(float delay)
    {
        Timer = delay;
    }

    public void TryGetParried(float angle, GameObject boss = null)
    {
        if (Timer < parryTime && Timer > 0)
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
        if (_timer > 0) 
        { 
            _timer -= Time.deltaTime;

            if (_timer <= 0)
            {
                if ( _isParried )
                {
                    _isParried = false;
                    return;
                }
                //Ÿ�� ���� ����.
                Vector2 offset = transform.parent.GetComponent<SpriteRenderer>().flipX ? _startingOffset + _collider.offset : new Vector2 ((_startingOffset.x + _collider.offset.x) * -1 , _startingOffset.y + _collider.offset.y);
                Collider2D[] playerCols = Physics2D.OverlapBoxAll((Vector2) transform.position + offset, _collider.size, LayerMask.GetMask("Player"));
                for (int i = 0; i < playerCols.Length; i++)
                {
                    if (playerCols[i].GetComponent<PlayerStats>() is PlayerStats stat)
                    {
                        //Debug.Log("Hit");
                        stat.TakeDamage(Damage);
                    }
                }
            }
        }
        //Debug.Log(_isParried);
    }

}
