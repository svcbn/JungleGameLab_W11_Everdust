using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character  : MonoBehaviour
{
    public int playerIndex;

    public Character Target;


    void Start()
    {
        if(Target == null)
        {
            Debug.LogWarning(" Target is null ");
        }

        GetDistance(Target.transform);
    }

    
    void GetDistance(Transform target)
    {
        float distance = Vector3.Distance(transform.position, target.position);
        Debug.Log($" distance : {distance} ");
    }

}
