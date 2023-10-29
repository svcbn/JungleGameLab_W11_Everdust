using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    Vector2 dir;

    float AimHorizontal;
    float AimVertical;

    void Update()
    {
        if(InputManager.Instance.AimButton)
        {
            AimHorizontal = InputManager.Instance.AimHorizontal;
            AimVertical   = InputManager.Instance.AimVertical;

            dir = new Vector2(AimHorizontal, AimVertical).normalized;
            
        }else{
            dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        }

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        transform.rotation = rotation;

    }
}