using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class ParryingTest : MonoBehaviour
{
    public Transform parryPosition;
    ParticleSystem parryingEffect;
    GameObject parryingEffectObject;
    BoxCollider2D _hitBox;
    [SerializeField] float _parryingTime = 0.2f;
    WaitForSecondsRealtime parriyngTime;
    private bool _isParry = false;

    float parryAngle = 0f;
    GameObject parryBoss = null;
    public bool IsParry
    {
        get
        {
            return _isParry;
        }
        set
        {
            _isParry = value;
            if(_isParry )
            {
                parryingEffect.Clear();
                parryingEffect.transform.position = parryPosition.position;
                parryingEffect.startRotation3D = new Vector3(0, 0, parryAngle );
                parryingEffect.Play();
                
                if(parryBoss != null){
                    float deltaDist = 5;
                    float deltaTime = 0.5f;
                    if(parryPosition.position.x <= parryBoss.transform.position.x){
                        parryBoss.transform.DOMoveX(parryBoss.transform.position.x + deltaDist, deltaTime);
                    }
                    else{
                        parryBoss.transform.DOMoveX(parryBoss.transform.position.x - deltaDist, deltaTime);
                    }
                }
            }
            else
            {

            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _hitBox = GetComponent<BoxCollider2D>();
        _hitBox.enabled = false;
        parriyngTime = new WaitForSecondsRealtime(_parryingTime);
        parryingEffectObject = Instantiate(Resources.Load<GameObject>("Prefabs/ParryingEffect"));
        parryingEffect = parryingEffectObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

        
        
    }

    public void TriggerParry(float angle, GameObject boss = null)
    {
        parryAngle = angle *Mathf.Deg2Rad ;
        parryBoss = boss;
        // 조건
        StartCoroutine(Parry());
        
    }

    IEnumerator Parry()
    {

        IsParry = true;
        //Debug.Log("parryOn");

        yield return parriyngTime;

        IsParry = false;
        //Debug.Log("parryOff");
    }

    

}
