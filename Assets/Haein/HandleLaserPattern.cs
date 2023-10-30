using System.Collections;
using UnityEngine;

public class HandleLaserPattern : MonoBehaviour
{
    public float widthOfLevel;
    public GameObject movingLeafSpike;

    public GameObject horizontalLaser;
    public GameObject verticalLaser;

    private int _createNum;
    private int _dir;

    private Coroutine _laserCR;

    public void StartLaserPattern(bool facingLeft)
    {
        _dir = facingLeft ? -1 : 1;
        if (_laserCR != null ) { StopCoroutine(_laserCR); }
        _laserCR = StartCoroutine(LaserPattern());
    }

    private IEnumerator LaserPattern()
    {
        var position = transform.position;
        
        yield return new WaitForSeconds(.1f);
        
        CreateMovingProjectile();
        var laser = Instantiate(verticalLaser, new Vector3(-widthOfLevel + widthOfLevel * (_dir + 1), -6f, 0f) + position, Quaternion.identity).GetComponent<HandleLaser>();
        laser.Dir = _dir * -1;
        
        yield return new WaitForSeconds(1.2f);
        
        Instantiate(horizontalLaser, new Vector3(-widthOfLevel, position.y, 0f) + position, Quaternion.identity);
    }
    private void CreateMovingProjectile()
    {
        if (movingLeafSpike is null) return;
        _createNum = 2;
        InvokeRepeating(nameof(CreateProjectile), 0f, 0.5f);

    }
        

    private void CreateProjectile()
    {
        const float distance = 5f;
        if (_createNum <= 0)
        {
            CancelInvoke(nameof(CreateProjectile));
            return;
        }

        var angle = (_createNum - 1) * (360f / 2);
        var radians = angle * Mathf.Deg2Rad;
        
        var xOffset = Mathf.Sin(radians) * distance;
        var zOffset = Mathf.Cos(radians) * distance;

        var createXPos = (_dir == 1) ? -40f : 10f;
        var createPos = new Vector3(createXPos, 17f, 0f);
        var spawnPosition = createPos + new Vector3(xOffset, 0f, zOffset);

        var newProjectile = Instantiate(movingLeafSpike, spawnPosition, Quaternion.identity);
        _createNum--;
    }

}
