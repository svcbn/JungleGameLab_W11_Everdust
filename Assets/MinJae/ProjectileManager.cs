using System;
using System.Collections;
using System.Collections.Generic;
using Myd.Platform;
using UnityEngine;
using Sirenix.OdinInspector;

public class ProjectileManager : MonoBehaviour
{
    public GameObject Enemy;
    

    public GameObject magicCirclePrefab;
    List<GameObject> magicCircles = new List<GameObject>();
    [ReadOnly][SerializeField]private int activeProjectileCount = 0;

    public float timeBetweenProj = 0.3f;

    private List<Vector3> offSetsProj = new List<Vector3>{
                                                new Vector3(3,3,0),
                                                new Vector3(3,-3,0),
                                                new Vector3(-3,-3,0),
                                                new Vector3(-3,3,0) };

    [SerializeField] private float _projectileOffsetDistance = 5f;

    bool is4ProjAttacking;

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

    public bool CanUse4ProjAttack()
    {
        return !is4ProjAttacking;
    }



    IEnumerator DisplayProjectileCO( float timeBetweenProj = 0.5f)
    {
        Debug.Log("DisplayProjectileCO");
        if(magicCirclePrefab == null) { Debug.LogWarning(" magicCirclePrefab is null "); yield break; }
        
        List<Vector3> randomOrderVs = ShuffleOrder(offSetsProj);

        for(int i=0; i<randomOrderVs.Count; i++)
        {
            if( activeProjectileCount >= randomOrderVs.Count ){ break; }
            activeProjectileCount++;

            CreateProjectile(randomOrderVs[i], order: i);
            yield return new WaitForSeconds(timeBetweenProj);
        }
    }

    void CreateProjectile(Vector3 posOffset, int order)
    {
        GameObject magicCircle = Instantiate(magicCirclePrefab, transform);

        magicCircle.GetComponent<MagicCircle>().Init(this, posOffset, order);
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


    public List<Vector3> ShuffleOrder(List<Vector3> vList)
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
