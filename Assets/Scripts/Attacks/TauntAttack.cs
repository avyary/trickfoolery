using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntAttack : Attack
{
    public override void Deactivate()
    {
        base.Deactivate();
        _renderer.enabled = false;
    }
}
