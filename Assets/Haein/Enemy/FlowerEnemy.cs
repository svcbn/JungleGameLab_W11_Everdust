using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerEnemy : Enemy
{
    private bool canFlip = true;
    private SpriteRenderer spriteRenderer;
    //private GameObject weaknessCircle;
    private Vector3 circlePos;
    private Animator animator;
    private int attackMode;
    private int moveDir;
    private float moveSpeed = 2f;
    private float delay = 0f;
    
    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        weaknessCircle = GetComponentInChildren<HandleWeaknessCircleAnimation>().gameObject;
        circlePos = weaknessCircle.transform.localPosition;
        animator = GetComponentInChildren<Animator>();
        attackMode = 0;
    }
    
    protected override void Update()
    {
        base.Update();
        if (delay > 0f)
        {
            delay -= Time.deltaTime;
        }
        else
        {
            delay = 0f;
        }
        //move X
        if (canFlip && Mathf.Abs(PlayerManager.Instance.player.transform.position.y - transform.position.y) < 2f)
        {
            moveDir = spriteRenderer.flipX ? 1 : -1;
            transform.Translate(moveDir * moveSpeed * Time.deltaTime, 0f, 0f);
        }
        if (PlayerManager.Instance.player != null)
        {
            if (canFlip)
            {
                if (transform.position.x < PlayerManager.Instance.player.transform.position.x)
                {
                    spriteRenderer.flipX = true;
                    circlePos.x = Mathf.Abs(circlePos.x) * (-1);
                    if (TryGetComponent(out HandleWeaknessCircle wc))
                    {
                        wc.isFlip = true;
                    }
                    weaknessCircle.transform.localPosition = circlePos;
                }
                else
                {
                    circlePos.x = Mathf.Abs(circlePos.x);
                    if (TryGetComponent(out HandleWeaknessCircle wc))
                    {
                        wc.isFlip = false;
                    }
                    weaknessCircle.transform.localPosition = circlePos;
                    spriteRenderer.flipX = false;
                }
            }

            if (attackMode == 0) //가시 올라오는 곻격
            {
                if (canFlip && Mathf.Abs(PlayerManager.Instance.player.transform.position.y - transform.position.y) < 2f)
                {
                    if (Mathf.Abs(PlayerManager.Instance.player.transform.position.x - transform.position.x) < 7f)
                    {
                        canFlip = false;
                        animator.SetTrigger("Attack");
                    }
                }
            }
            else //차징 공격
            {
                if (canFlip && Mathf.Abs(PlayerManager.Instance.player.transform.position.y - transform.position.y) < 5f)
                {
                    if (Mathf.Abs(PlayerManager.Instance.player.transform.position.x - transform.position.x) < 15f)
                    {
                        canFlip = false;
                        animator.SetTrigger("Charge");
                    }
                }
            }
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("FlowerAttack") || animator.GetCurrentAnimatorStateInfo(0).IsName("FlowerCharging"))
            {
                if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && delay <= 0f)
                {
                    delay = 1f;
                    StartCoroutine(HandleAnimationEnd());
                }
            }
            
        }
    }

    public IEnumerator HandleAnimationEnd()
    {
        yield return new WaitForSeconds(1f);
        canFlip = true;
        attackMode = attackMode == 0 ? 1 : 0;
    }
}
