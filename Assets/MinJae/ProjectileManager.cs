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

    [SerializeField] private List<float> degrees = new List<float>{ 45f, 135f, 225f, 315f };
    [SerializeField] private float _projectileOffsetDistance = 6.7f;
    [SerializeField] private float timeBetweenProj = 0.5f;
    // MagicCircleData 의 값들과 함께 볼 것

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
            if(Enemy == null){ Debug.Log("Enemy is null "); return; }

            Enemy.TryGetComponent<MagicMissile>(out MagicMissile magicMissile);
            if(magicMissile == null){ Debug.Log("No <MagicMissile> Component in Enmey"); return; }

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
        
        List<int> randomOrderInts = ShuffleOrder(degrees);

        for(int i=0; i<randomOrderInts.Count; i++)
        {
            if( activeProjectileCount >= randomOrderInts.Count ){ break; }

            CreateProjectile( degrees[ randomOrderInts[i] ], order: i);
            yield return new WaitForSeconds(timeBetweenProj);
        }
    }

    void CreateProjectile(float angle, int order)
    {
        activeProjectileCount++;

        GameObject magicCircle = Instantiate(magicCirclePrefab, transform);

        magicCircle.GetComponent<MagicCircle>().Init(this, angle, _projectileOffsetDistance, order);
        magicCircles.Add(magicCircle);
    }

    
    public void EraseProjectile(GameObject magicCircle)
    {
        magicCircles.Remove(magicCircle);
        activeProjectileCount--;

        if( activeProjectileCount <= 0){ 
            Debug.Log( $" activeProjectileCount : {activeProjectileCount}" );
            is4ProjAttacking = false; 

        }
        magicCircle.SetActive(false);
        Destroy(magicCircle);
    }

    public List<int> ShuffleOrder(List<float> degrees_)
    {
        System.Random rand = new System.Random();

        List<int> intList = new List<int>();

        for (int i=0; i< degrees_.Count; i++)
        {
            intList.Add(i);
        }

        List<int> intRandomList = new List<int>();

        //intList의 값을 랜덤으로 뽑아서 intRandomList에 넣는다.
        for (int i = intList.Count - 1; i >= 0; i--)
        {
            int j = rand.Next(i + 1); // 0부터 i까지의 범위 내에서 랜덤한 정수를 선택
            intRandomList.Add(intList[j]);
            intList.RemoveAt(j);
        }

        return intRandomList;
    }
}
