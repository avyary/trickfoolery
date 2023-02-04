using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchAttack : Attack
{
    protected override void Start() {
        startupTime = 0.5f;
        activeTime = 0.25f;
        recoveryTime = 1;
        damage = 100;
        range = 2;
        stunTime = 1;
        base.Start();
    }
}