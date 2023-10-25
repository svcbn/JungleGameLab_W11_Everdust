using System;
using UnityEngine;

public class HandleWeaknessCircle : MonoBehaviour
{
    public bool isFlip = false;
    public GameObject weaknessCircle;
    private SpriteRenderer spriteRenderer;
    private Animator weaknessCircleAnimator;

    private void Start()
    {
        weaknessCircleAnimator = weaknessCircle.GetComponent<Animator>();
        weaknessCircle.GetComponent<SpriteRenderer>().flipX = isFlip;
    }

    private void Update()
    {
        if (PlayerManager.Instance.player != null)
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
    }
    
    public bool IsWeaknessAttacked()
    {
        if (GetComponent<HandleWeaknessCircle>().isFlip) //왼쪽으로 때려야 하는 경우
        {
            if (PlayerManager.Instance.player.transform.position.x < transform.position.x)
            {
                return true;
            }
        }
        else //오른쪽으로 때려야 하는 경우
        {
            if (PlayerManager.Instance.player.transform.position.x > transform.position.x)
            {
                return true;
            }
        }
        return false;
    }
}