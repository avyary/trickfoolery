using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchAttack : Attack
{
    protected override void Start() {
        startupTime = 0.5f;
        activeTime = 0.5f;
        recoveryTime = 1;
        damage = 10;
        range = 2.5f;
        base.Start();
    }
}