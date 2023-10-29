using System.Collections;
using System.Collections.Generic;
using Myd.Platform;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;



public class GuidedBulletMover : Mover
{
    [Header("GuidedBulletMover")] // 유도탄 관련 속성
    private Vector2 startPos;
    private Vector2 delta1;
    private Vector2 delta2;

    public Transform target;

    public float eta = 1.0f;

    private float timer = 0f;
    private float reta;

    private bool initialized  = false;
    [HideInInspector]public float bezierDelta  = 10.0f; // MagicMissileData 에서 받아옴
    [HideInInspector]public float bezierDelta2 = 10.0f; // MagicMissileData 에서 받아옴

    public float refindRadius = 20f;

    public int damage;

    public void Init(int _damage)
    {
        damage = _damage;

        reta = 1 / eta;
        startPos = transform.position;
        delta1 = SetDelta(startPos);
        if (target == null)
        {
            FindNewTarget();
        }
        if (target != null)
        {
            delta2 = SetDelta(target.transform.position);
        }

        initialized = true;
    }

    private Vector2 SetDelta(Vector2 org)
    {
        float x, y;
        x = Mathf.Cos(Random.Range(0f, 360f) * Mathf.Deg2Rad) * bezierDelta + org.x;
        y = Mathf.Sin(Random.Range(0f, 360f) * Mathf.Deg2Rad) * bezierDelta + org.y;
        return new Vector2(x, y);
    }


    private float GetDistance(Transform target)
    {
        float distance = Vector3.Distance(transform.position, target.position);
        return distance;
    }

    private void FindNewTarget()
    {
        LayerMask mask = LayerMask.GetMask("Player");// attacker.playerLayer;
        Collider2D[] t = Physics2D.OverlapCircleAll(transform.position, refindRadius, mask);

        if( t == null)
        {
            Debug.Log("GuidedBulletMover: t is null");
            Destroy(gameObject);
            return;
        }


        List<Collider2D> validTargets = new List<Collider2D>();
        foreach (var col in t)
        {
            Debug.Log($"GuidedBulletMover: {col.gameObject.name}");


            validTargets.Add(col);
            
        }

        if (validTargets.Count > 0)
        {
            Collider2D c = validTargets[Random.Range(0, validTargets.Count)];
            target = c.gameObject.transform;
        }
        else
        {
            Destroy(gameObject);
        }
        //Collider2d[] Physics2D.OverlapCircle(transform.position, 7f);
    }

    public override void MovePhase0() // 멈추기까지
    {
        // 실제 곡선 궤적을 그리는 부분
        body.position = new Vector2(
            Bezier(timer, startPos.x, delta1.x, delta2.x, target.transform.position.x),
            Bezier(timer, startPos.y, delta1.y, delta2.y, target.transform.position.y));
        timer += Time.fixedDeltaTime * reta;

        //Debug.Log(timer + ", (" + direction.x + ", " + direction.y + ")");
        //direction = Vector3.Slerp(direction.normalized, (target.position - transform.position).normalized, slerpCorrection);

        // float distance = GetDistance(target);
        // if( distance < 6f){
        //     SetMovePhase(MovePhase.phase1);
        // }
    }
    
    public override void MovePhase1() // 부웅 커지는 파트
    {
        base.MovePhase1();

        transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);

        
        if( transform.localScale.x > 2.5f )
        {
            SetMovePhase(MovePhase.phase2);
        }
    }

    public override void MovePhase2() // 유저에게 돌격
    {
        base.MovePhase2();



    }


    public override void AfterMove()
    {
        if (target == null || !target.gameObject.activeSelf)
        {
            FindNewTarget();
        }
        if (!initialized) return;


    }

    private float Bezier(float t, float a, float b, float c, float d)
    {
        return Mathf.Pow((1 - t), 3) * a
            + Mathf.Pow((1 - t), 2) * t * b * 3
            + Mathf.Pow(t, 2) * (1 - t) * c * 3
            + Mathf.Pow(t, 3) * d;
    }

    void OnDrawGizmos()
    {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, refindRadius);
    }

}
