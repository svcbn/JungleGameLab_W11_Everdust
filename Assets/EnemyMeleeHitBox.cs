using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeHitBox : MonoBehaviour
{
    public static float parryTime = 0.1f;

    private float _timer;
    private BoxCollider2D _collider;
    private Vector2 _startingOffset;
    private bool _isParried = false;

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

    public void TryGetParried()
    {
        if (Timer < parryTime) _isParried = true;
    }

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _startingOffset = transform.localPosition;
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
                //타격 판정 순간.
                Vector2 offset = transform.parent.GetComponent<SpriteRenderer>().flipX ? _startingOffset + _collider.offset : new Vector2 ((_startingOffset.x + _collider.offset.x) * -1 , _startingOffset.y + _collider.offset.y);
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
    }

}
