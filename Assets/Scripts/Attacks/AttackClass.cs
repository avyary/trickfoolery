using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    public float startupTime { get; set; }
    public float activeTime { get; set; }
    public float recoveryTime { get; set; }
    public int damage { get; set; }
    public float range { get; set; }

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
