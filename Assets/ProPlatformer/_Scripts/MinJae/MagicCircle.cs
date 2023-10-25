using System.Collections;
using System.Collections.Generic;
using Myd.Platform;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MagicCircle : MonoBehaviour
{
    private Vector2 delta1;
    private Vector2 delta2;
    [HideInInspector]public float bezierDelta  = 0f; // MagicMissileData 에서 받아옴
    [HideInInspector]public float bezierDelta2 = 0f; // MagicMissileData 에서 받아옴



    Player player;
    Vector3 playerPos;
    Vector3 targetPos;
    Vector3 posOffset;

    bool followPlayer = true;
    bool waitTileShoot = false;


    float startTimer = 0;
    float shootTimer = 0;

    const float shotTime = 2f;

    public float shakeMagnitude = 0.1f; // 떨림의 강도

    private Vector3 originalPosition;

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
        startTimer += Time.deltaTime;

        playerPos = player.GetPlayerPosition();

        if(followPlayer){
            
            transform.position = playerPos + posOffset;

            if( startTimer > shotTime )
            {
                followPlayer = false;
                waitTileShoot = true;

                targetPos = playerPos;
                
                originalPosition = transform.position;

            }
        }else if(waitTileShoot)
        {
            shootTimer += Time.deltaTime;
            Shake();

            if(shootTimer > shotTime)
            {
                waitTileShoot = false;
            }
        }
        else
        {
            // targetPos 으로 돌격 
            MoveShoot();
            

        }


        
    }
    void MoveShoot(){

        float moveSpeed = 3f;

        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if(transform.position == targetPos)
        {
            Destroy(gameObject);
        }
    }


   void Shake()
    {
        transform.position = originalPosition + Random.insideUnitSphere * shakeMagnitude;
    }

}

