using System.Collections;
using System.Collections.Generic;
using Myd.Platform;
using Sirenix.OdinInspector.Editor.Validation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;


public class MagicCircle : Projectile
{
    ProjectileManager projM;

	private MagicCircleData _data;

    [SerializeField]private TMP_Text textOrder;

    Transform player;
    [ReadOnly][SerializeField]private Vector3 playerPos;
    [ReadOnly][SerializeField]private Vector3 targetPos;
    Vector3 posOffset;

    bool followPlayer  = true;
    bool waitTileShoot = false;


    float startTimer = 0;
    float shootTimer = 0;

    private Vector3 originalPosition;
    private Vector3 shootDirection;
    
    private bool isInit = false;

	private void LoadDataSO()
	{
		string pathDataSO = "Data/MagicCircleData"; // in Resource folder
		_data = Resources.Load<MagicCircleData>(pathDataSO);

		if(_data == null){
			Debug.LogWarning(" Data Load Fail ");
		}else{
			Debug.Log(" Data Load Success ");
		}
	}

    protected override void Start()
    {
        base.Start();
        MaxHp = 1;
        player = FindObjectOfType<PlayerRenderer>().transform;
    }


    public void Init(ProjectileManager projM_, Vector3 posOffset_, int order)
    {
		if(_data == null )
		{
			LoadDataSO();
		}

        projM     = projM_;
        posOffset = posOffset_;

        textOrder.text = order.ToString();



        isInit = true;
    }
    protected override void Update()
    {
        base.Update();
        if(!isInit) return;

        startTimer += Time.deltaTime;

        playerPos = player.position;

        if(followPlayer){
            
            transform.position = playerPos + posOffset;

            if( startTimer > _data.shotTime )
            {
                followPlayer = false;
                waitTileShoot = true;

                targetPos = playerPos;
                
                originalPosition = transform.position;

                shootDirection = (targetPos - transform.position).normalized;

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
            // shootDirection 으로 돌격 
            MoveShoot();
        }


        if(startTimer > _data.destroyTime)
        {
            EraseProjectile();
        }

    }


    void MoveShoot()
    {
        Vector3 nextPosition = transform.position + shootDirection * _data.moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, nextPosition) >= _data.shootDistance)
        {
            EraseProjectile();
        }
        else
        {
            transform.position = nextPosition;
            
        }
    }

   void Shake()
    {
        transform.position = originalPosition + Random.insideUnitSphere * _data.shakeMagnitude;
    }

    public override void EraseProjectile()
    {
        projM.EraseProjectile(gameObject);
    }
}

