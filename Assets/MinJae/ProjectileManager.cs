using System.Collections;
using System.Collections.Generic;
using Myd.Platform;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    Player player;

    public GameObject Enemy;
    

    public GameObject magicCirclePrefab;
    List<GameObject> magicCircles = new List<GameObject>();

    [SerializeField]private List<Vector3> offSetsProj = new List<Vector3>{
                                                            new Vector3(3,3,0),
                                                            new Vector3(3,-3,0),
                                                            new Vector3(-3,-3,0),
                                                            new Vector3(-3,3,0) };


    int activeProjectileCount = 0;
    static int allProjCnt = 0;

    public void Init(Player player_)
    {
        player = player_;
    }


    void Start()
    {
        
    }

    void Update()
    {
        if( Input.GetKeyDown(KeyCode.Q) ) // for test
        {
            Debug.Log("Down Key Q");

            StartCoroutine(DisplayProjectileCO(waitTime:0.1f));
        }

        if( Input.GetKeyDown(KeyCode.R) ) // for test
        {
            Debug.Log("Down Key R ");

            if(Enemy == null)
            {
                Debug.Log(" Enemy is null ");
                return;
            }

            Enemy.TryGetComponent<MagicMissile>(out MagicMissile magicMissile);
            if(magicMissile == null)
            {
                Debug.Log(" No <MagicMissile> Component  in Enmey ");
                return;
            }

            Enemy.GetComponent<MagicMissile>().Init();
            Enemy.GetComponent<MagicMissile>().Execute();
        }

    }


    IEnumerator DisplayProjectileCO( float waitTime = 0.5f)
    {

        Debug.Log("DisplayProjectileCO");


        for(int i=0; i<4; i++)
        {
            DisplayProjectile();
            yield return new WaitForSeconds(waitTime);
        }

    }


    // todo: 생기는 순서 랜덤으로.
    // 구체간의 소환시간 파라미터 설정가능하도록
    void DisplayProjectile()
    {
        if( activeProjectileCount >= 4 ){ return; }

        activeProjectileCount++;
        allProjCnt++;


        if(magicCirclePrefab == null)
        {
            Debug.Log(" magicCirclePrefab is null ");
            return;
        }

        GameObject magicCircle = Instantiate(magicCirclePrefab);

        
        int idx = (allProjCnt-1) % 4;

        magicCircle.GetComponent<MagicCircle>().Init(this, player, offSetsProj[idx]);
        magicCircles.Add(magicCircle);
        
    }

    public void EraseProjectile(GameObject magicCircle)
    {
        magicCircles.Remove(magicCircle);
        activeProjectileCount--;
        Destroy(magicCircle);
    }

}
