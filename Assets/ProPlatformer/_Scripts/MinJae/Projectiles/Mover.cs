using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Windows;


public enum MovePhase
{
    phase0,
    phase1,
    phase2
}


public class Mover : MonoBehaviour
{
	public bool _canMove = true;
    public bool CanMove
    {
        get { return _canMove; }
        set { _canMove = value; }
    }


    public MovePhase movePhase = MovePhase.phase1;


    [Header("속도 관련")]
    public float speed;
    public float accel;
    public Vector2 direction;
    public Vector2 angularAccel;

    protected Rigidbody2D body;

    public Vector2 lastInput = new Vector2(0,-1);


    private void FixedUpdate()
    {
        if (direction != Vector2.zero)
        {
            lastInput = direction;
        }

        if (CanMove)
        {
            Move();
            AfterMove();
        }
        
    }



    public void Move()
    {
        if (body == null)
        {
            body = GetComponent<Rigidbody2D>();
        }

        switch(movePhase)            
        {
        case MovePhase.phase0:
            MovePhase0();
            break;
        case MovePhase.phase1:
            MovePhase1();
            break;
        case MovePhase.phase2:
            MovePhase2();
            break;

        }

    }

    public void CalculateDelta()
    {
        //속도, 가속도, 각속도 계산
        speed     += accel        * Time.fixedDeltaTime; 
        direction += angularAccel * Time.fixedDeltaTime;
    }


    public void SetMovePhase(MovePhase nextPhase)
    {
        movePhase = nextPhase;

        Debug.Log("SetMovePhase to : " + movePhase);
    }

    public virtual void MovePhase0()
    {
        // 사실상 안쓰이는 부분. 천천히 느리게 움직.
        body.MovePosition(body.position + speed * Time.fixedDeltaTime * direction.normalized);
        body.velocity = direction.normalized * speed;

        CalculateDelta();
    }


    public virtual void MovePhase1() {}
    public virtual void MovePhase2() {}

    public virtual void AfterMove() { }
}
