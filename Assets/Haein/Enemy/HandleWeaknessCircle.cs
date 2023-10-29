using System;
using UnityEngine;

public class HandleWeaknessCircle : MonoBehaviour
{
    public enum WEAKTYPE
    {
        ALWAYS = 100,
        DISTANCE,
        ONLYCHARGING,
        CONTINUOUSWEAKNESS,
        DISTANCEDELAY,
    }
    
    public bool isFlip = false;
    public GameObject weaknessCircle;
    private SpriteRenderer spriteRenderer;
    private Animator weaknessCircleAnimator;
    public int weaknessCircleType = (int)WEAKTYPE.DISTANCE;
    public int currentWeaknessNum = 0;
    public Vector3 originalCirclePos;
    public float appearDelay = 0f;

    public Vector2[] weaknessCirclePosArr;
    
    private void Start()
    {
        weaknessCircleAnimator = weaknessCircle.GetComponent<Animator>();
        weaknessCircle.GetComponent<SpriteRenderer>().flipX = isFlip;
        ChangeWeaknessPosition();
        originalCirclePos = weaknessCircle.transform.localPosition;
    }

    private void Update()
    {
        weaknessCircle.GetComponent<SpriteRenderer>().flipX = isFlip;
        if (PlayerManager.Instance.player != null)
        {
            if (weaknessCircleType == (int)WEAKTYPE.ALWAYS)
            {
                weaknessCircle.SetActive(true);
            }
            else if (weaknessCircleType == (int)WEAKTYPE.DISTANCE)
            {
                float distance = Vector3.Distance(PlayerManager.Instance.player.transform.position, transform.position);
                if (distance < 15f)
                {
                    weaknessCircle.SetActive(true);
                }
                else
                {
                    weaknessCircle.SetActive(false);
                }
            }
            else if (weaknessCircleType == (int)WEAKTYPE.ONLYCHARGING)
            {
                
            }
            else if (weaknessCircleType == (int)WEAKTYPE.DISTANCEDELAY)
            {
                if (appearDelay > 0f)
                {
                    appearDelay -= Time.deltaTime;
                }

                if (appearDelay <= 0f)
                {
                    float distance = Vector3.Distance(PlayerManager.Instance.player.transform.position, transform.position);
                    if (distance < 15f)
                    {
                        weaknessCircle.SetActive(true);
                    }
                    else
                    {
                        weaknessCircle.SetActive(false);
                    }
                }
                else
                {
                    weaknessCircle.SetActive(false);
                }
            }
            else
            {
                
            }
        }
    }
    
    public bool IsWeaknessAttacked()
    {
        if (isFlip) //왼쪽으로 때려야 하는 경우
        {
            if (PlayerManager.Instance.player.transform.position.x < weaknessCircle.transform.position.x)
            {
                return true;
            }
        }
        else //오른쪽으로 때려야 하는 경우
        {
            if (PlayerManager.Instance.player.transform.position.x > weaknessCircle.transform.position.x)
            {
                return true;
            }
        }
        return false;
    }

    public void ChangeWeaknessPosition()
    {
        if (currentWeaknessNum > weaknessCirclePosArr.Length - 1 && weaknessCirclePosArr.Length > 0)
        {
            if (TryGetComponent(out Boss b))
            {
                b.CancelMagicCircle();
            }
            ResetWeaknessPosition();
            return;
        }

        if (TryGetComponent(out Boss _boss))
        {
            if (currentWeaknessNum == 0 && !_boss.GetComponent<Boss>().GetCurrentFlipXIsTurnedOn())
            {
                Vector3 _scale = weaknessCircle.transform.parent.localScale;
                _scale.x *= -1;
                isFlip = true;
                weaknessCircle.transform.parent.localScale = _scale;
            }
            
            if (currentWeaknessNum == 1)
            {
                isFlip = !isFlip;
            }
        }
        
        if( weaknessCirclePosArr.Length > currentWeaknessNum ){ 
            Vector3 targetPos = weaknessCirclePosArr[currentWeaknessNum];
        
            /*
            if (_boss is not null)
            {
                targetPos = spriteRenderer.flipX == false
                    ? new Vector3(targetPos.x * -1, targetPos.y, targetPos.z)
                    : targetPos;
            }
            */
            weaknessCircle.transform.localPosition = targetPos;
        }
    }
    
    public void ResetWeaknessPosition()
    {
        currentWeaknessNum = 0;
        weaknessCircle.transform.localPosition = weaknessCirclePosArr[0];
    }
}