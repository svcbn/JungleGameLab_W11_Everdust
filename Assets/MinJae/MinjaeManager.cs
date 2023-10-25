using System.Collections;
using System.Collections.Generic;
using Myd.Platform;
using UnityEngine;

public class MinjaeManager : MonoBehaviour
{

    public GameObject p1;
    public GameObject p2;



    void Start()
    {
        
    }

    void Update()
    {
        if( Input.GetKeyDown(KeyCode.A) )
        {
            Debug.Log("Down Key A ");

            p1.GetComponent<MagicMissile>().Init();

        }

        if( Input.GetKeyDown(KeyCode.S) )
        {
            Debug.Log("Down Key S ");

            p1.GetComponent<MagicMissile>().Execute();

        }
    }
}
