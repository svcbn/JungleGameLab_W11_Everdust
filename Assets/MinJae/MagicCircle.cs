using System.Collections;
using System.Collections.Generic;
using Myd.Platform;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MagicCircle : MonoBehaviour
{
    ProjectileManager projM;

	private MagicCircleData _data;

    Player player;
    Vector3 playerPos;
    Vector3 targetPos;
    Vector3 posOffset;

    bool followPlayer  = true;
    bool waitTileShoot = false;


    float startTimer = 0;
    float shootTimer = 0;

    private Vector3 originalPosition;

	private void LoadDataSO()
	{
		string pathDataSO = "Data/MagicCircleData"; // in Resource folder
		_data = Resources.Load<MagicCircleData>(pathDataSO);

		if(_data == null)
		{
			Debug.LogWarning(" Data Load Fail ");
		}else{
			Debug.Log(" Data Load Success ");
		}
	}

    void Start()
    {
        
    }

    public void Init(ProjectileManager projM_, Player player_, Vector3 posOffset_)
    {
		if(_data == null )
		{
			LoadDataSO();
		}

        projM     = projM_;
        player    = player_;
        posOffset = posOffset_;

    }


    void Update()
    {
        startTimer += Time.deltaTime;

        playerPos = player.GetPlayerPosition();

        if(followPlayer){
            
            transform.position = playerPos + posOffset;

            if( startTimer > _data.shotTime )
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

            if(shootTimer > _data.shotTime)
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


        transform.position = Vector3.MoveTowards(transform.position, targetPos, _data.moveSpeed * Time.deltaTime);

        if(transform.position == targetPos)
        {
            projM.EraseProjectile(gameObject);
        }
    }


   void Shake()
    {
        transform.position = originalPosition + Random.insideUnitSphere * _data.shakeMagnitude;
    }

}

