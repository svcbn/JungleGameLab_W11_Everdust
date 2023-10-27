using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPhysics : MonoBehaviour
{
    public float minFallSpeed;
    private Rigidbody2D _rb2d;

    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (_rb2d.velocity.y < 0 && _rb2d.velocity.y > minFallSpeed) _rb2d.velocity = Vector2.up * minFallSpeed;
    }
}
