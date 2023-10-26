using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleAttackEvent : MonoBehaviour
{
    public GameObject _hitBox;
    public GameObject _attackBox;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateHitBox()
    {
        int dir = GetComponent<SpriteRenderer>().flipX ? 1 : -1;
        Instantiate(_hitBox, transform.position + new Vector3(dir * 2.3f, -.8f, 0f), Quaternion.identity);
    }

    public void createAttackBox()
    {
        int dir = GetComponent<SpriteRenderer>().flipX ? 1 : -1;
        Instantiate(_attackBox, transform.position + new Vector3(dir * 2.3f, -.8f, 0f), Quaternion.identity);
    }
}
