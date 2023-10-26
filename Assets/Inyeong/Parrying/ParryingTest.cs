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

    public bool IsParry { get; private set; } = false;

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
        
    }

    public void TriggerParry()
    {
        // Á¶°Ç

        StartCoroutine(Parry());
    }

    IEnumerator Parry()
    {
        IsParry = true;
        Debug.Log("parryOn");

        yield return parriyngTime;

        IsParry = false;
        Debug.Log("parryOff");
    }

    
    private void OnCollisionEnter2D(Collision2D other)
    {

        Debug.Log("fdssfds");
        animator.SetTrigger("Parrying");
    }
}
