using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class Laser : MonoBehaviour
{
     private LineRenderer _lineRenderer;
     private EdgeCollider2D _edgeCollider;
     private bool hasDamaged = false;

    [Header("Ray")]
    [SerializeField] private float rayDistance; // 레이저 최대 길이
    [SerializeField] private Vector3 rayDirection; // 레이저 방향
    [SerializeField] private float laserBeforeSize = 0.5f;
    [SerializeField] private float laserAfterSize = 2f;
    private float laserSize;
    private Vector3 startOffset = Vector3.zero;
    [SerializeField] private float delayTime; // 발사 전 딜레이 시간
    [SerializeField] private float laserTime; // 레이저 발사 델타 시간
    [SerializeField] private float animVelocity; // 레이저 커지는 속도

    public bool isGroundBlocking = false; // 땅에 막히는지

    bool isRayOn = false; // 레이저 켜졌는지
    Vector3 rayStart;
    Vector3 rayEnd;

    float time = 0;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _edgeCollider = GetComponent<EdgeCollider2D>();

        laserSize = laserBeforeSize;
        _lineRenderer.SetWidth(laserSize,laserSize);

        SetRayPosition();
    }

    private void FixedUpdate()
    {
        time += Time.deltaTime;
        if(time >= delayTime){
            laserSize += animVelocity;
            laserSize = laserAfterSize < laserSize ? laserAfterSize : laserSize;
            _lineRenderer.SetWidth(laserSize,laserSize);
            isRayOn = true;
            if(time >= delayTime + laserTime){
                Destroy(gameObject);
            }
        }

        SetRayPosition();
        if(isGroundBlocking)
            HitRay();
    }
    private void HitRay()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(rayStart, rayDirection, rayDistance);
        Vector2[] points = _edgeCollider.points;
        if (hit.Length != 0)
        {
            foreach (var item in hit)
            {
                if (item.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    if (rayDirection.y == 0){
                        float colliderSizeX = item.point.x - rayStart.x;
                        rayEnd.x = item.point.x;

                        _lineRenderer.SetPosition(1, rayEnd);
                        points[1].x = colliderSizeX - 0.5f;
                        points[1].y = points[0].y;
                        _edgeCollider.points = points;
                    }
                    else{
                        float colliderSizeY = item.point.y - transform.position.y + startOffset.y;
                        rayEnd.y = item.point.y;

                        _lineRenderer.SetPosition(1, rayEnd);
                        points[1].y = colliderSizeY - 0.5f;
                        points[1].x = points[0].x;
                        _edgeCollider.points = points;
                    }
                    return;
                }
            }
        }   
    }

    public static bool IsHitRay(Laser laser, GameObject obj)
    {
        return laser.IsLaserHitRay(obj);
    }
    public bool IsLaserHitRay(GameObject obj)
    {
        if(!isRayOn) return false;
        RaycastHit2D[] hit = Physics2D.RaycastAll(rayStart, rayDirection, rayDistance);
        if (hit.Length != 0)
        {
            foreach (var item in hit)
            {
                if (item.collider.gameObject == obj)
                {
                    return true;
                }
            }
        }   
        return false;
    }

    void SetRayPosition(){
        rayStart = transform.position + startOffset;
        rayEnd = rayStart + rayDistance * rayDirection;
        _lineRenderer.SetPosition(0, rayStart);
        _lineRenderer.SetPosition(1, rayEnd);
        
        Vector2[] points = _edgeCollider.points;
        if (rayDirection.y == 0){
            points[1].x = rayDistance;
        }
        else{
            points[1].y = rayDistance;
        }
        _edgeCollider.points = points;

    }
    
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && hasDamaged == false)
        {
            hasDamaged = true;
            //플레이어 데미지 스크립트 추가
            PlayerManager.Instance.player.GetComponent<PlayerStats>().TakeDamage(10);
        }
    }
}
