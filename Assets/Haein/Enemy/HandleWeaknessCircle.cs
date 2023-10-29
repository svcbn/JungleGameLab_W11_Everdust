using System;
using System.Linq;
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
    private Animator weaknessCircleAnimator;
    public int weaknessCircleType = (int)WEAKTYPE.DISTANCE;
    public int currentWeaknessNum = 0;
    public Vector3 originalCirclePos;
    public float appearDelay = 0f;
    private Boss _boss;

    public Vector2[] weaknessCirclePosArr;
    private SpriteRenderer _weaknessSpriteRenderer;

    private void Awake()
    {
        _weaknessSpriteRenderer = weaknessCircle.GetComponent<SpriteRenderer>();
        _boss = GetComponent<Boss>();
    }

    private void Start()
    {
        weaknessCircleAnimator = weaknessCircle.GetComponent<Animator>();
        weaknessCircle.GetComponent<SpriteRenderer>().flipX = isFlip;
        originalCirclePos = weaknessCircle.transform.localPosition;
    }

    private void Update()
    {
        _weaknessSpriteRenderer.flipX = isFlip;
        if (PlayerManager.Instance.player is not null)
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
                    float distance = Vector3.Distance(PlayerManager.Instance.player.transform.position,
                        transform.position);
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
        //약점 모두 타격 시
        if (currentWeaknessNum > weaknessCirclePosArr.Length - 1 && weaknessCirclePosArr.Length > 0)
        {
            _boss?.CancelMagicCircle();
            ResetWeaknessPosition();
            return;
        }

        if (_boss is not null)
        {
            if (currentWeaknessNum == 1)
            {
                isFlip = !isFlip;
            }
        }
        
        //print(currentWeaknessNum);
        if (weaknessCirclePosArr.Length > currentWeaknessNum)
        {
            Vector3 targetPos = weaknessCirclePosArr[currentWeaknessNum];

            if (_boss is not null)
            {
                var inverse = _boss.GetFlipX() ? 1 : -1;
                //print(inverse);
                targetPos.x *= inverse;
            }
            weaknessCircle.transform.localPosition = targetPos;
        }
    }

    public void Init_BossOnly()
    {
        if (_boss is null) return;

        isFlip = _boss.GetFlipX();
        ChangeWeaknessPosition();
    }

    public void ResetWeaknessPosition()
    {
        currentWeaknessNum = 0;
        weaknessCircle.transform.localPosition = weaknessCirclePosArr[0];
    }
}