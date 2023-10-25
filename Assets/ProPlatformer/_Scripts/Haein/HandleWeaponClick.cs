using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleWeaponClick : MonoBehaviour
{
    private Animator animator; // Animator 컴포넌트를 참조하기 위한 변수

    void Start()
    {
        // 이 스크립트가 연결된 게임 오브젝트의 Animator 컴포넌트 가져오기
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭 감지
        {
            // "Poke" 애니메이션을 실행합니다. Animator의 트리거 매개변수에 따라 변경할 수 있습니다.
            animator.SetTrigger("Poke");
        }
    }
}