using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Attacks;
using UnityEngine;

// public struct ShockwaveRange
// {
//     public const float NormalWidth = 2.0f;
//     public const float NormalLength = 3.0f;
//     public const float AggroWidth = 4.0f;
//     public const float AggroLength = 6.0f;
// }

public class ShockwaveEnemy : Enemy
{
    [SerializeField]
    private AudioClip attackSound;

    // private Dictionary<string, float> _normalShockwaveZone = new Dictionary<string, float>();
    // private Dictionary<string, float> _aggroShockwaveZone = new Dictionary<string, float>();
    

    // protected void ShockwaveZone()
    // {
    //     Debug.Log("The Normal Shockwave Zone:");
    //     foreach (KeyValuePair<string, float> kvp in _normalShockwaveZone)
    //         Debug.Log(kvp.Key + ": " + kvp.Value.ToString("0.0"));
    //
    //     Debug.Log("The Aggro Shockwave Zone:");
    //     foreach (KeyValuePair<string, float> kvp in _aggroShockwaveZone)
    //         Debug.Log(kvp.Key + ": " + kvp.Value.ToString("0.0"));
    // }

    protected override void Start()
    {
        base.Start();
        maxHealth = 100;
        health = maxHealth;
        maxAnger = 1;
        
        GetEnemyStatus("ShockwaveEnemy");
        //
        // _normalShockwaveZone.Add("width", ShockwaveRange.NormalWidth);
        // _normalShockwaveZone.Add("length", ShockwaveRange.NormalLength);
        // _aggroShockwaveZone.Add("width", ShockwaveRange.AggroWidth);
        // _aggroShockwaveZone.Add("length", ShockwaveRange.AggroLength);

    }

    void Update()
    {
        switch (state)
        {
            case EnemyState.Patrolling:
                // if (agent.isStopped) 
                // {
                //     agent.isStopped = false;
                // }
                // Patrol();
                if (!fow.active)    
                {
                    fow.active = true;
                    StartCoroutine(fow.FindPlayer(moveSpeed, PlayerFound));
                }
                break;
            
            case EnemyState.Tracking:
                float dist = Vector3.Distance(gameObject.transform.position, player.transform.position);
                if ( dist <= basicAttack.range)
                {
                    Debug.Log("Shockwave detected the player");
                    // StopEnemy();
                    ShockWaveAttack();
                }
                // } else 
                // {
                //     GoToTarget();
                // }
                state = EnemyState.Patrolling;
                break;
            
            case EnemyState.Active:
                // StartCoroutine(Freeze(5));
                break;
        }
    }

    protected IEnumerator Attack(AngryShockwaveAttack attackObjA, BasicShockwaveAttack attackObjB)
    {
        Attack attackObj;
        if (isAngy)
        {
            attackObj = attackObjA;
        }
        else
        {
            attackObj = attackObjB;
        }
        
        // trigger attack animation here
        state = EnemyState.Startup;
        yield return new WaitForSeconds(attackObj.startupTime);
        
        // there's probably a better way to handle the below (& its repetitions)
        if (state == EnemyState.Dead || state == EnemyState.Stunned) {
            yield break;
        }

        state = EnemyState.Active;
        yield return new WaitForSeconds(attackObj.activeTime);
        
        if (isAngy)
        {
            attackObjA.Activate();
            // yield return new WaitForSeconds(attackObj.activeTime);
            attackObjA.SetAngryShockwaveAttack();
        }
        else
        {
            attackObjB.Activate();
            // yield return new WaitForSeconds(attackObj.activeTime);
            attackObjB.SetBasicShockwaveAttack();
        }

        if (state == EnemyState.Dead || state == EnemyState.Stunned) {
            yield break;
        }

        state = EnemyState.Recovery;
        attackObj.Deactivate();  // deactivate attack collider
        yield return new WaitForSeconds(attackObj.recoveryTime);

        if (state == EnemyState.Dead || state == EnemyState.Stunned) {
            yield break;
        }
        
        state = EnemyState.Patrolling;
        Debug.Log("I am friendly!");
    }
    
    
    public void ShockWaveAttack()
    {
        Debug.Log("Shockwave Attacking Begins");
        
        AngryShockwaveAttack angryShockwaveAttack = (AngryShockwaveAttack)angyAttack;
        BasicShockwaveAttack basicShockwaveAttack = (BasicShockwaveAttack)basicAttack;
        
        StartCoroutine(Attack(angryShockwaveAttack, basicShockwaveAttack));
        Debug.Log("Shockwave Attacking");
    }
    
    IEnumerator Freeze(float sec) {
        // Save the object's current position
        Vector3 originalPosition = transform.position;

        // Set the object's position to its current position, effectively "freezing" it
        transform.position = originalPosition;

        // Wait for 3 seconds
        yield return new WaitForSeconds(sec);
    }
    
}
