using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleLaserPattern : MonoBehaviour
{
    public GameObject movingLeafSpike;

    public GameObject horizontalLaser;
    public GameObject verticalLaser;

    private int createNum;
    private int dir;

    void Start()
    {
        StartCoroutine(LaserPattern());
    }

    IEnumerator LaserPattern()
    {
        CheckDirection();
        yield return new WaitForSeconds(.1f);
        CreateMovingProjectile();
        Instantiate(verticalLaser, new Vector3(-50f + 50f * (dir + 1), -6f, 0f), Quaternion.identity);
        yield return new WaitForSeconds(1.2f);
        Instantiate(horizontalLaser, new Vector3(-50f, 8f, 0f), Quaternion.identity);
    }
    
    private void CheckDirection()
    {
        if (transform.position.x < 0) dir = 1;
        else dir = -1;
    }
    
    public void CreateMovingProjectile()
    {
        if (movingLeafSpike == null) return;
        createNum = 2;
        InvokeRepeating("CreateProjectile", 0f, 0.5f);

    }

    private void CreateProjectile()
    {
        float distance = 5f;
        if (createNum <= 0)
        {
            CancelInvoke("CreateProjectile");
            return;
        }

        float angle = (createNum - 1) * (360f / 2);
        float radians = angle * Mathf.Deg2Rad;
        
        float xOffset = Mathf.Sin(radians) * distance;
        float zOffset = Mathf.Cos(radians) * distance;

        float createXPos = (dir == 1) ? -40f : 50f;
        Vector3 createPos = new Vector3(createXPos, 17f, 0f);
        Vector3 spawnPosition = createPos + new Vector3(xOffset, 0f, zOffset);

        GameObject newProjectile = Instantiate(movingLeafSpike, spawnPosition, Quaternion.identity);
        createNum--;
    }

}
