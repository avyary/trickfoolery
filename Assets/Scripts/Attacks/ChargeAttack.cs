using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttack : Attack
{
    protected override void Start() {
        startupTime = 1;
        activeTime = 0.5f;
        recoveryTime = 0.5f;
        damage = 10;
        range = 3f;
        base.Start();
    }
}
