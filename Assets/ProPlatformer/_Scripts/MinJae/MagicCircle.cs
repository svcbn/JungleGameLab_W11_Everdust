using System.Collections;
using System.Collections.Generic;
using Myd.Platform;
using UnityEngine;

public class MagicCircle : MonoBehaviour
{
    Player player;
    Vector3 playerPos;
    Vector3 posOffset;

    void Start()
    {
        
    }

    public void Init(Player player_, Vector3 posOffset_)
    {
        player    = player_;
        posOffset = posOffset_;
    }


    void Update()
    {
        playerPos = player.GetPlayerPosition();

        transform.position = playerPos + posOffset;
    }
}
