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
    Vector3 circlePos;

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


    public void Init(ProjectileManager projM_, float angle, float distance, int order)
    {
		if(_data == null )
		{
			LoadDataSO();
		}

        textOrder.text = (order + 1).ToString(); // 구체 표시되는 숫자
        textOrder.gameObject.SetActive(false);  

        projM     = projM_;


        SetRedboxAngle(angle, distance);

        circlePos = CalculatePoint(playerPos, angle, distance);

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
            
            transform.position = playerPos + circlePos;

            if( startTimer > _data.followEndTime )
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

            if(shootTimer > _data.shootTime)
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

        if (Vector3.Distance(transform.position, nextPosition) >= _data.longestDistance)
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

    void SetRedboxAngle(float angle, float distance)
    {
        redbox.transform.rotation = Quaternion.Euler(0,0,angle);        

        switch(angle)
        {
            case 45:
            redbox.transform.position += new Vector3(-2,-2,0);
            break;
            case 135:
            redbox.transform.position += new Vector3(2,-2,0);
            break;
            case 225:
            redbox.transform.position += new Vector3(2,2,0);
            break;
            case 315:
            redbox.transform.position += new Vector3(-2,2,0);
            break;

        }
    }


    public static Vector3 CalculatePoint(Vector3 startPoint, float angle, float distance)
    {
        // 각도를 라디안으로 변환
        float radian = angle * Mathf.Deg2Rad;

        // 좌표 계산
        float x = distance * Mathf.Cos(radian);
        float y = distance * Mathf.Sin(radian);

        Vector3 endPoint = new Vector3(startPoint.x + x, startPoint.y + y, startPoint.z);

        return endPoint;
    }

}

