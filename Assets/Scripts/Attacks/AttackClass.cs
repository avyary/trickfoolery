using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Attack : MonoBehaviour
{
    public float startupTime { get; set; }
    public float activeTime { get; set; }
    public float recoveryTime { get; set; }
    public int damage { get; set; }
    public int stunTime { get; set; }
    public float range { get; set; }

    private Collider _collider;
    private MeshRenderer _renderer;

    private string[] collisionTags = {"Player", "Enemy"};

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

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().TakeHit(damage, stunTime);
        }
        else if (other.gameObject.tag == "Player")
        {
        }
        // if (collisionTags.Any(other.gameObject.tag.Contains))
        // {
        //     other.gameObject.TakeHit(damage);
        // }
    }
}
