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
    }
    
    public bool isFlip = false;
    public GameObject weaknessCircle;
    private SpriteRenderer spriteRenderer;
    private Animator weaknessCircleAnimator;
    public int weaknessCircleType = (int)WEAKTYPE.DISTANCE;
    public int currentWeaknessNum = 0;
    public Vector3 originalCirclePos;
    
    private void Start()
    {
        weaknessCircleAnimator = weaknessCircle.GetComponent<Animator>();
        weaknessCircle.GetComponent<SpriteRenderer>().flipX = isFlip;
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
        weaknessCircle.transform.localPosition = new Vector3(weaknessCircle.transform.localPosition.x, weaknessCircle.transform.localPosition.y - .3f, weaknessCircle.transform.localPosition.z);
    }
    
    public void ResetWeaknessPosition()
    {
        weaknessCircle.transform.localPosition = originalCirclePos;
    }
}