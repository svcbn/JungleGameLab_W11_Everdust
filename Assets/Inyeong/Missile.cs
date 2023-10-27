using System.Collections;
using System.Collections.Generic;
using Myd.Platform;
using Sirenix.OdinInspector.Editor.Validation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Missile : Projectile
{
    protected override void Start()
    {
        base.Start();
        MaxHp = 1;
    }

}

