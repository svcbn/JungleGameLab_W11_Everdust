using System.Collections;
using UnityEngine;

public class ReflectMissile : Projectile
{
    [SerializeField] float attackLifeTime = 3f;
    [SerializeField] float attackPower = 1f;

    Rigidbody2D _rigid;
    protected override void Awake(){
        base.Awake();
        _rigid = GetComponent<Rigidbody2D>();
    }
    protected override void Start()
    {
        base.Start();
        MaxHp = 100;
    }
    
    protected override void Update()
    {
        base.Update();
    }


    
    public override void TakeHit(bool hitWeakness = false, float attackAngle = 0f)
    {
        float angle = -attackAngle;
        Debug.Log(angle);
        Vector3 direction =  new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle), 0);
        Debug.Log(direction);
        _rigid.velocity = attackPower * direction;
        StartCoroutine(CountLifeTime());
        
    }


    IEnumerator CountLifeTime(){
        float timer = 0;
        while(true){
            timer += Time.deltaTime;
            if(timer >= attackLifeTime)
                Destroy(gameObject);
            yield return null;
        }
    }

}

