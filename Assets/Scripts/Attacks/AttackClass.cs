using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Attack : MonoBehaviour
{
    [SerializeField]
    float _startupTime;
    [SerializeField]
    float _activeTime;
    [SerializeField]
    float _recoveryTime;
    [SerializeField]
    int _damage;
    [SerializeField]
    float _stunTime;
    [SerializeField]
    float _range;

    public float startupTime { get; set; }
    public float activeTime { get; set; }
    public float recoveryTime { get; set; }
    public int damage { get; set; }
    public float stunTime { get; set; }
    public float range { get; set; }

    protected Collider _collider;
    protected MeshRenderer _renderer;

    private string[] collisionTags = {"Player", "Enemy"};

    protected virtual void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
        _renderer.enabled = false;
        _collider.enabled = false;

        startupTime = _startupTime;
        activeTime = _activeTime;
        recoveryTime = _recoveryTime;
        damage = _damage;
        stunTime = _stunTime;
        range = _range;
    }

    public virtual void Activate()
    {
        _collider.enabled = true;
    }

    public virtual void Deactivate()
    {
        _collider.enabled = false;
    }

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().TakeHit(transform.parent.GetComponent<Enemy>(), damage, stunTime);
        }
        else if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerMovement>().TakeHit(damage, transform.parent.GetComponent<Enemy>());
        }
        // if (collisionTags.Any(other.gameObject.tag.Contains))
        // {
        //     other.gameObject.TakeHit(damage);
        // }
    }
}
