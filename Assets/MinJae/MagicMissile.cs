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
			//Debug.Log(" Data Load Success ");
		}
	}


	public void Init()
	{
		if(_data == null )
		{
			LoadDataSO();
		}

		Owner = gameObject;
		if(Owner == null){ Debug.LogWarning(" Owner is null "); return; }

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
        if (cols.Length == 0) // 타겟이 OverlapCircleAll에 잡히지 않았을때
        {
			Debug.Log("타겟이 OverlapCircleAll에 잡히지 않음");
            for (int i = 0; i < _data.missileCount; i++)
            {
                GuidedBulletMover g = Instantiate(_data.missilePrefab, Owner.transform.position, Quaternion.identity);

                g.target = null;
                g.bezierDelta  = _data.bezierDelta;
                g.bezierDelta2 = _data.bezierDelta2;
                g.Init(_data.Damage);
            }
            yield break;
        }

		List<Collider2D> validTargets = new List<Collider2D>();
		foreach (var col in cols)
		{
			validTargets.Add(col);
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
                g.Init(_data.Damage);

				colCount += 1;            
			}
		}


		// 후딜
		yield return new WaitForSeconds(AfterDelay);
	}

}

