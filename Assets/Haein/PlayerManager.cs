using System.Collections;
using System.Collections.Generic;
using Myd.Platform;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject player;
    public bool canMove = true;
    public float bondTime = 2f;
    
    private static PlayerManager instance = null;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
    public static PlayerManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    void Update()
    {
        if (player != null)
        {
            if (bondTime > 0f)
            {
                canMove = false;
                player.GetComponent<PlayerRenderer>().ChainEffect.SetActive(true);
                bondTime -= Time.deltaTime;
            }
            else
            {
                player.GetComponent<PlayerRenderer>().ChainEffect.SetActive(false);
                canMove = true;
            }
        }
        
        #if UNITY_EDITOR // 속박 테스트
            
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            bondTime = 2f;
        }
        
        #endif
    }
    
    
}
