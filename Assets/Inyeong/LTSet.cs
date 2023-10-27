using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Light Trigger Set
public class LTSet : MonoBehaviour
{
    public LightTrigger[] triggerList;
    public GameObject[] platforms;

    public float limitTriggerTime = 5f;

    bool isAllTrigger = false;
    bool isTimerStart = false;
    float timer = 0f;

    void Start()
    {
        SetAllPlatforms(false);
        SetAllTriggerOff();
    }

    void Update()
    {
        if(isTimerStart){
            timer += Time.deltaTime;
            if(timer >= limitTriggerTime){
                SetTimerOff();
                SetAllTriggerOff();
            }
        }

        CheckAllTrigger();
        if(isAllTrigger) {
            SetAllPlatforms(true);
            SetTimerOff();
        }
    }

    void SetAllPlatforms(bool isActivate){
        foreach (GameObject p in platforms)
        {
            p.SetActive(isActivate);
        }
    }
    bool CheckAllTrigger(){
        bool tmpAllTrigger = true;
        foreach (LightTrigger trigger in triggerList)
        {
            if(!trigger.isOnTrigger) tmpAllTrigger =  false;
            if(!isAllTrigger)
                isTimerStart = true;
        }
        isAllTrigger = tmpAllTrigger;
        return isAllTrigger;
    }
    void SetAllTriggerOff(){
        foreach (LightTrigger trigger in triggerList)
        {
            if(trigger.isOnTrigger)
                trigger.OffTrigger();
        }
    }

    void SetTimerOff(){
        timer = 0;
        isTimerStart = false;
    }
}
