using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParryingTest : MonoBehaviour
{
    public Animator animator;
    BoxCollider2D _hitBox;
    [SerializeField] float _parryingTime = 0.2f;
    WaitForSecondsRealtime parriyngTime;
    private bool _isParry = false;

    public bool IsParry
    {
        get
        {
            return _isParry;
        }
        set
        {
            _isParry = value;
            if(_isParry )
            {
                // when doing parry
            }
            else
            {
                // when end of parry
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _hitBox = GetComponent<BoxCollider2D>();
        _hitBox.enabled = false;
        parriyngTime = new WaitForSecondsRealtime(_parryingTime);
    }

    // Update is called once per frame
    void Update()
    {
        
        //if (Input.GetMouseButtonDown(0))
        //{
        //    animator.SetTrigger("Parrying");
        //}
    }

    public void TriggerParry()
    {
        // ����

        StartCoroutine(Parry());
        
    }

    IEnumerator Parry()
    {
        animator.SetTrigger("Parrying");

        IsParry = true;
        //Debug.Log("parryOn");

        yield return parriyngTime;

        IsParry = false;
        //Debug.Log("parryOff");
    }

    

}
