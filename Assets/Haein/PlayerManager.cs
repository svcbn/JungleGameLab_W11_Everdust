using System.Collections;
using System.Collections.Generic;
using Myd.Platform;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject player;
    public bool canMove = true;
    private float bondTime = 0f;
    
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
    }

    public void SetPlayerBond(float time)
    {
        bondTime = time;
    }
    
    public void ShowText(string str)
    {
        if (player != null)
        {
            Vector3 positionWithRandomOffset = player.transform.position + new Vector3(0f, 2f, -1f);
            GameObject damageTextPrefab = Resources.Load<GameObject>("Prefabs/UI/DamageText");
            GameObject damageText = Instantiate(damageTextPrefab, positionWithRandomOffset, Quaternion.identity);
            damageText.GetComponent<MoveAndDestroy>()._text = str;
        }
    }
    
}
