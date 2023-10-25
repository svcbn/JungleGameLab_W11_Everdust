using System.Collections;
using System.Collections.Generic;
using Myd.Platform;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    Player player;

    public GameObject Enemy;
    

    public GameObject magicCirclePrefab;

    List<Vector3> offSets = new List<Vector3>{
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
            Debug.Log("Q");

            DisplayProjectile();
        }

        if( Input.GetKeyDown(KeyCode.R) ) // for test
        {
            Debug.Log("Down Key R ");
            Enemy.GetComponent<MagicMissile>().Init();
            Enemy.GetComponent<MagicMissile>().Execute();
        }

    }


    void DisplayProjectile()
    {
        if( activeProjectileCount >= 4 ){ return; }

        activeProjectileCount++;
        allProjCnt++;

        GameObject magicCircle = Instantiate(magicCirclePrefab);

        
        int idx = (allProjCnt-1) % 4;

        magicCircle.GetComponent<MagicCircle>().Init(this, player, offSets[idx]);
        magicCircles.Add(magicCircle);
        
    }

    public void EraseProjectile(GameObject magicCircle)
    {
        magicCircles.Remove(magicCircle);
        activeProjectileCount--;
        Destroy(magicCircle);
    }

}