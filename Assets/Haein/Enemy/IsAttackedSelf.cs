using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IsAttackedSelf : MonoBehaviour
{
    public bool isAttacked = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Weapon"))
        {
            
        }
    }
}
