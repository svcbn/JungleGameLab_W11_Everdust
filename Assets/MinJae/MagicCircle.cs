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

    [SerializeField]private GameObject redbox;

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

    bool isBlinking = false;

	private void LoadDataSO()
	{
		string pathDataSO = "Data/MagicCircleData"; // in Resource folder
		_data = Resources.Load<MagicCircleData>(pathDataSO);

		if(_data == null){
			Debug.LogWarning(" Data Load Fail ");
		}else{
			//Debug.Log(" Data Load Success ");
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
        textOrder.text = (order + 1).ToString() ;

        projM     = projM_;
        posOffset = posOffset_;


        SetRedboxAngle(posOffset_);


        redbox.SetActive(false);

        isInit = true;
    }

    protected override void Update()
    {
        //base.Update();
    }

    void FixedUpdate()
    {
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
            BlinkRedBox();

            if(shootTimer > _data.shotTime)
            {
                waitTileShoot = false;
                redbox.SetActive(false);
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

    void BlinkRedBox() // 깜박이는 레드 박스
    {
        isBlinking = !isBlinking; // 상태 토글
        redbox.SetActive(isBlinking);

    }

    void SetRedboxAngle(Vector3 posOffset_)
    {
        if(posOffset_ == new Vector3(-3,-3,0))
        {
            redbox.transform.position += new Vector3(1,1,0);
            redbox.transform.rotation = Quaternion.Euler(0,0,-135);
        }else if(posOffset_ == new Vector3(-3,3,0))
        {
            redbox.transform.position += new Vector3(1,-1,0);
            redbox.transform.rotation = Quaternion.Euler(0,0,-45);
            
        }else if(posOffset_ == new Vector3(3,-3,0))
        {
        redbox.transform.position += new Vector3(-1,1,0);
            redbox.transform.rotation = Quaternion.Euler(0,0,-45);
        }else if(posOffset_ == new Vector3(3,3,0))
        {
            redbox.transform.position += new Vector3(-1,-1,0);
            redbox.transform.rotation = Quaternion.Euler(0,0,-135);
        }
    }

}

