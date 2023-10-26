using System.Collections;
using System.Collections.Generic;
using Myd.Platform;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    Player player;

    public GameObject Enemy;
    

    public GameObject magicCirclePrefab;

    [SerializeField]private List<Vector3> offSetsProj = new List<Vector3>{
                                                            new Vector3(3,3,0),
                                                            new Vector3(3,-3,0),
                                                            new Vector3(-3,-3,0),
                                                            new Vector3(-3,3,0) };
    List<GameObject> magicCircles = new List<GameObject>();


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

            DisplayProjectile();
        }

        if( Input.GetKeyDown(KeyCode.R) ) // for test
        {
            Debug.Log("Down Key R ");

            if(Enemy == null)
            {
                Debug.Log(" Enemy is null ");
                return;
            }

            Enemy.GetComponent<MagicMissile>().Init();
            Enemy.GetComponent<MagicMissile>().Execute();
        }

    }


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
