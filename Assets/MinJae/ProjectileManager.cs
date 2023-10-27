using System;
using System.Collections;
using System.Collections.Generic;
using Myd.Platform;
using UnityEngine;
using Sirenix.OdinInspector;

public class ProjectileManager : MonoBehaviour
{
    Player player;

    public GameObject Enemy;
    

    public GameObject magicCirclePrefab;
    List<GameObject> magicCircles = new List<GameObject>();
    [ReadOnly][SerializeField]private int activeProjectileCount = 0;

    public float timeBetweenProj = 0.3f;

    [SerializeField]
    private List<Vector3> offSetsProj = new List<Vector3>{
                                            new Vector3(3,3,0),
                                            new Vector3(3,-3,0),
                                            new Vector3(-3,-3,0),
                                            new Vector3(-3,3,0) };

    bool is4ProjAttacking;

    public void Init(Player player_)
    {
        player = player_;
    }


    void Update()
    {
        if( Input.GetKeyDown(KeyCode.Q) ) // for test
        {
            Debug.Log("Down Key Q");
            Start4ProjAttack();
        }

        if( Input.GetKeyDown(KeyCode.R) ) // for test
        {
            Debug.Log("Down Key R ");

            return;
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

    // Use this function 
    public void Start4ProjAttack()
    {
        if(is4ProjAttacking){ return; }
        is4ProjAttacking = true;
        StartCoroutine(DisplayProjectileCO(timeBetweenProj));
    }



    IEnumerator DisplayProjectileCO( float timeBetweenProj = 0.5f)
    {
        Debug.Log("DisplayProjectileCO");
        if(magicCirclePrefab == null) { Debug.LogWarning(" magicCirclePrefab is null "); yield break; }
        
        List<Vector3> randomOrderVs = Shuffle(offSetsProj);

        for(int i=0; i<randomOrderVs.Count; i++)
        {
            if( activeProjectileCount >= randomOrderVs.Count ){ break; }
            activeProjectileCount++;

            DisplayProjectile(randomOrderVs[i], order: i);
            yield return new WaitForSeconds(timeBetweenProj);
        }
    }

    void DisplayProjectile(Vector3 posOffset, int order)
    {
        GameObject magicCircle = Instantiate(magicCirclePrefab);

        magicCircle.GetComponent<MagicCircle>().Init(this, player, posOffset, order);
        magicCircles.Add(magicCircle);
    }

    public void EraseProjectile(GameObject magicCircle)
    {
        magicCircles.Remove(magicCircle);
        activeProjectileCount--;

        if( activeProjectileCount == 0){ 
            Debug.Log( $" activeProjectileCount : {activeProjectileCount}" );
            is4ProjAttacking = false; 

        }

        Destroy(magicCircle);
    }


    public List<Vector3> Shuffle(List<Vector3> vList)
    {
        System.Random rand = new System.Random();

        List<Vector3> newVList = new List<Vector3>(vList);

        for (int i = newVList.Count - 1; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            Vector3 temp = newVList[i];
            newVList[i] = newVList[j];
            newVList[j] = temp;
        }

        return newVList;
    }
}
