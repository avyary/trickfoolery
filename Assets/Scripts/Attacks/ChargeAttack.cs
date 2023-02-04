using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttack : Attack
{
    protected override void Start() {
        startupTime = 0.5f;
        activeTime = 0.5f;
        recoveryTime = 0.5f;
        damage = 100;
        range = 3f;
        stunTime = 1;
        base.Start();
    }
}
