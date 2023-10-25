using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleWeaponClick : MonoBehaviour
{
    public bool isPoke = false;
    public float pokeTime = 0f;
    public float pokeTimeMax = 0.1f;
    private Animator animator; // Animator 컴포넌트를 참조하기 위한 변수

    void Start()
    {
        // 이 스크립트가 연결된 게임 오브젝트의 Animator 컴포넌트 가져오기
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Poke");
            pokeTime = pokeTimeMax;
            isPoke = true;
        }
        if (pokeTime > 0)
        {
            pokeTime -= Time.deltaTime;
        }
        else
        {
            isPoke = false;
        }
    }
}