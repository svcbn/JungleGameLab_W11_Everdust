using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryingTest : MonoBehaviour
{
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    
    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("fdssfds");
        animator.SetTrigger("Parrying");
    }
}
