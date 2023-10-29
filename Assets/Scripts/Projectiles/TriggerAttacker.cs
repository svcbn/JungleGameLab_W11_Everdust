using UnityEngine;

public class TriggerAttacker : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy      owner = GetComponent<GuidedBulletMover>().owner;
        int       damage = GetComponent<GuidedBulletMover>().damage;

        // if (collision.GetComponent<Character>()             == null             ){ return; }
        // if (collision.GetComponent<Character>().playerIndex == owner.playerIndex){ return; }
        if (collision.gameObject == owner.gameObject){ return; } // 동작안함
        Debug.Log("dasdfsafd");
        if (collision.IsTouchingLayers(LayerMask.GetMask("Player")) == false){ return; }
    
        Debug.Log("TODO: trigger Do Damage ");

        //Managers.Stat.GiveDamage(1 - owner.playerIndex, damage);
        //owner.Target.GetComponent<CharacterStatus>().SetKnockbackEffect(1f, 30f, transform.position);

        Destroy(this.gameObject);
    }
}
