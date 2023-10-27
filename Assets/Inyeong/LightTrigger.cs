using System.Collections;
using System.Collections.Generic;
using Myd.Platform;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class LightTrigger : Trigger
{
    public Color offTriggerColor = Color.white;
    public Color onTriggerColor = Color.red;

    SpriteRenderer _spriteRenderer;

    public bool isOnTrigger;
    
    protected override void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if(isOnTrigger) _spriteRenderer.color = onTriggerColor; 
        else _spriteRenderer.color = offTriggerColor; 
    }
    public override void OnTrigger() {
        _spriteRenderer.color = onTriggerColor;
        isOnTrigger = true;
    }
    public void OffTrigger() {
        _spriteRenderer.color = offTriggerColor;
        isOnTrigger = false;
    }
}
