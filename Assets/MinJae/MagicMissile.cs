using System.Collections;
using System.Collections.Generic;
using Myd.Platform;
using UnityEngine;



public class MagicMissile : MonoBehaviour
{
	private MagicMissileData _data;
	float AfterDelay = 1f;

	GameObject Owner;


	private void LoadDataSO()
	{
		string pathDataSO = "Data/MagicMissileData"; // in Resource folder
		_data = Resources.Load<MagicMissileData>(pathDataSO);

		if(_data == null)
		{
			Debug.LogWarning(" Data Load Fail null ");
		}else{
			Debug.Log(" Data Load Success ");
		}
	}


	public void Init()
	{
		if(_data == null )
		{
			LoadDataSO();
		}

		Owner = gameObject; //GetComponent<GameObject>()
		if(Owner == null)
		{
			Debug.LogWarning(" Owner is null ");
		}else{
			Debug.Log( $" Owner : {Owner.name} ");
		}


	}

	public void Execute()
	{
		StartCoroutine(ExecuteImplCo());

	}

	public IEnumerator ExecuteImplCo()
	{
		Debug.Log("ExecuteImplCo");


        int colCount = 0;
        Collider2D[] cols = Physics2D.OverlapCircleAll(Owner.transform.position, 40f, _data.targetLayer);
        if (cols.Length == 0) // 타겟이 없을때 
        {
            for (int i = 0; i < _data.missileCount; i++)
            {
                GuidedBulletMover g = Instantiate(_data.missilePrefab, Owner.transform.position, Quaternion.identity);

                g.target = null;
                g.bezierDelta  = _data.bezierDelta;
                g.bezierDelta2 = _data.bezierDelta2;
                g.Init(Owner, _data.Damage);
            }
            yield break;
        }

		List<Collider2D> validTargets = new List<Collider2D>();
		foreach (var col in cols)
		{
			if (col.GetComponent<Character>() != Owner)
			{
				validTargets.Add(col);
			}
		}

		if( validTargets.Count > 0)
		{
			for(int i = 0; i < _data.missileCount; i++)
			{
				if (i%validTargets.Count==0) { colCount = 0; }
				GuidedBulletMover g = Instantiate(_data.missilePrefab, Owner.transform.position, Quaternion.identity);

				g.target = validTargets[colCount].transform;
				g.bezierDelta  = _data.bezierDelta;  // 상대 원
				g.bezierDelta2 = _data.bezierDelta2; // 나 원
                g.Init(Owner, _data.Damage);

				colCount += 1;            
			}
		}


		// 후딜
		yield return new WaitForSeconds(AfterDelay);
	}

	
	public bool CheckCanUse()
	{
		//bool isEnemyInRange = Vector2.Distance(Owner.Target.transform.position, Owner.transform.position) <= _data.range;

		//return isEnemyInRange;
		return true;
	}

	// private void OnDrawGizmos() 
	// {
	// 	Gizmos.color = new Color(255f / 255f, 182f / 255f, 193f / 255f); // 연분홍색
	// 	Vector3 checkboxPos = Owner.transform.position;
	// 	Gizmos.DrawWireCube(checkboxPos + (Vector3)_data.CheckBoxCenter, (Vector3)_data.CheckBoxSize);	
	// }
}

