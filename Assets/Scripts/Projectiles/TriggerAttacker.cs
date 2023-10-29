using UnityEngine;

public class TriggerAttacker : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int targetLayer = LayerMask.GetMask("Player");

        int       damage = GetComponent<GuidedBulletMover>().damage;

        if ( !collision.IsTouchingLayers(targetLayer) ){ return; }
    
        //Debug.Log("TODO: trigger Do Damage ");


        Destroy(gameObject);
    }
}
