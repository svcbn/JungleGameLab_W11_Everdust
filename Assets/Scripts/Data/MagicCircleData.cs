
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = nameof(MagicCircleData), menuName = "ScriptableObjects/" + nameof(MagicCircleData))]
public class MagicCircleData : ScriptableObject
{

    [Header("Damage")]
	[SerializeField] private int _damage;


	public int Damage => _damage;


    [Header("Detail")]
	public GameObject missilePrefab;
    

    public float shakeMagnitude; // 떨림의 강도
	public float longestDistance;

    public float moveSpeed; // 탄환이 날아가는 속도

	[Header("Timer")]
	public float followEndTime;
    public float shootTime; 
	public float destroyTime;

	//ProjectileManager 의 _projectileOffsetDistance, timeBetweenProj 값과 함께 볼것

}
