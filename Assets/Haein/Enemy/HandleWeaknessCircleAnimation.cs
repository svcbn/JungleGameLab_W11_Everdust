using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleWeaknessCircleAnimation : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Animator>().SetTrigger("Appear");
    }
}
