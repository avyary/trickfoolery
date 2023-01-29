using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    public virtual float startupTime { get; set; }
    public virtual float activeTime { get; set; }
    public virtual float recoveryTime { get; set; }
    public virtual int damage { get; set; }

    private Collider _collider;
    private MeshRenderer _renderer;

    protected virtual void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
        _renderer.enabled = false;
        _collider.enabled = false;

    }

    public void Activate()
    {
        _renderer.enabled = true;
        _collider.enabled = true;
    }

    public void Deactivate()
    {
        _renderer.enabled = false;
        _collider.enabled = false;
    }
}

public class ChargeAttack : Attack
{
    protected override void Start() {
        startupTime = 1;
        activeTime = 2;
        recoveryTime = 0.5f;
        damage = 10;
        base.Start();
    }
}
