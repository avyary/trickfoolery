using System.Collections;
using System.Collections.Generic;
using Attacks;
using UnityEngine;

public struct ShockwaveRange
{
    public const float NormalWidth = 2.0f;
    public const float NormalLength = 3.0f;
    public const float AggroWidth = 4.0f;
    public const float AggroLength = 6.0f;
}

public class ShockwaveEnemy : Enemy
{
    [SerializeField]
    private AudioClip attackSound;
    
    private Dictionary<string, float> _normalShockwaveZone = new Dictionary<string, float>();
    private Dictionary<string, float> _aggroShockwaveZone = new Dictionary<string, float>();
    

    protected void ShockwaveZone()
    {
        Debug.Log("The Normal Shockwave Zone:");
        foreach (KeyValuePair<string, float> kvp in _normalShockwaveZone)
            Debug.Log(kvp.Key + ": " + kvp.Value.ToString("0.0"));

        Debug.Log("The Aggro Shockwave Zone:");
        foreach (KeyValuePair<string, float> kvp in _aggroShockwaveZone)
            Debug.Log(kvp.Key + ": " + kvp.Value.ToString("0.0"));
    }

    protected override void Start()
    {
        base.Start();
        GetEnemyStatus("ShockwaveEnemy");
        if (anger == 0)
        {
            isAngy = false;
        }
        else
        {
            isAngy = true;
        }
        
        _normalShockwaveZone.Add("width", ShockwaveRange.NormalWidth);
        _normalShockwaveZone.Add("length", ShockwaveRange.NormalLength);
        _aggroShockwaveZone.Add("width", ShockwaveRange.AggroWidth);
        _aggroShockwaveZone.Add("length", ShockwaveRange.AggroLength);

    }

    Dictionary<string, float> Update()
    {
        switch (state)
        {
            case EnemyState.Passive:
                TestBehaviors.Rotate(gameObject, moveSpeed); // replace with better movement
                    if (!fow.active)    
                {
                    fow.active = true;
                    StartCoroutine(fow.FindPlayer(moveSpeed, PlayerFound));
                }
                break;
            
            case EnemyState.Tracking:
                TestBehaviors.MoveToPlayer(gameObject, player, moveSpeed);
                float dist = Vector3.Distance(gameObject.transform.position, player.transform.position);
                if ( dist <= basicAttack.range)
                {
                    StopEnemy();
                    return ShockWaveAttack();
                } else 
                {
                    GoToTarget();
                }
                state = EnemyState.Passive;
                break;
            
            case EnemyState.Active:
                TestBehaviors.MoveForward(gameObject, 1);
                break;
        }
        return new Dictionary<string, float>();
    }

    public Dictionary<string, float> ShockWaveAttack()
    {
        audioSource.PlayOneShot(attackSound);
        StartCoroutine(Attack(currentAttack));
        // Vector3 origin = gameObject.transform.position;
        if (this.isAngy)
        {
            return _aggroShockwaveZone;
        }
        else
        {
            // // Update the NormalShockwaveZone based on the origin
            // NormalShockwaveZone["width"] = ShockwaveRange.NormalWidth + origin.x;
            // NormalShockwaveZone["length"] = ShockwaveRange.NormalLength + origin.z;
            return _normalShockwaveZone;
        }
    }
}
