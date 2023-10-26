using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHandleWeaponClick : HandleWeaponClick
{

    public Animator _parryingAnimator;


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canPoke)
        {
            _parryingAnimator.SetTrigger("Parrying");
        }
    }

}