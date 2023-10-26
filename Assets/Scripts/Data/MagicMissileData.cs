
using UnityEngine;

[CreateAssetMenu(fileName = nameof(MagicMissileData), menuName = "ScriptableObjects/" + nameof(MagicMissileData))]
public class MagicMissileData : ScriptableObject
{

	[Header("OverlapBox")]
	[Header("Check")]
	[SerializeField] private Vector2 _checkBoxCenter;
	[SerializeField] private Vector2 _checkBoxSize;

	[Header("HitBox")]
	[SerializeField] private Vector2 _hitBoxCenter;
	[SerializeField] private Vector2 _hitBoxSize;


    [Header("Delay")]
	[SerializeField] private float _beforeDelay;
	[SerializeField] private float _afterDelay;



    [Header("Damage")]
	[SerializeField] private int _damage;

	public float BeforeDelay => _beforeDelay;
	public float AfterDelay  => _afterDelay;

	public Vector2 CheckBoxCenter => _checkBoxCenter;
	public Vector2 CheckBoxSize   => _checkBoxSize;

	public Vector2 HitBoxCenter => _hitBoxCenter;
	public Vector2 HitBoxSize   => _hitBoxSize;
	
	public int Damage => _damage;



    [Header("Magic Missle Detail")]

	public GuidedBulletMover missilePrefab;
    public LayerMask targetLayer; // Player Layer
    public float duration;
    public int missileCount;
    
    
    public float range;
    public float bezierDelta;
    public float bezierDelta2;
}
