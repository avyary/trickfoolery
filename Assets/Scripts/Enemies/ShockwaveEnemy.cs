using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveEnemy : Enemy
{
    protected override void Start() 
    {
        base.Start();
        GetEnemyStatus("ShockwaveEnemy");

    }

    void Update() 
    {   
            
        switch(state) 
        {
            case EnemyState.Passive:
                if (!fow.active)
                { 
                    fow.active = true;
                    StartCoroutine(fow.FindPlayer(moveSpeed, PlayerFound));
                }

                break;
            
            case EnemyState.Tracking:

                agent.SetDestination(player.transform.position);
                // should stop when it reaches its current attack distance from the player
                float dist = Vector3.Distance(gameObject.transform.position, player.transform.position);


                if (dist <= currentAttack.range) 
                {
                    Debug.Log("Shockwave detected the player"); 
                    agent.ResetPath();
                    StartCoroutine(ShockwaveAttack((BasicShockwaveAttackNew)currentAttack));
                }

                break;
            
            case EnemyState.Active:
                // if (!isAngy) 
                // {
                //     currentAttack.GetComponent<BasicShockwaveAttack>().StartAttack();
                // }
                break;
        }
    }


    protected IEnumerator ShockwaveAttack(BasicShockwaveAttackNew attackObj)
    {
        // trigger attack animation here
        state = EnemyState.Startup;
        // Debug.Log("Attacking Time");
        yield return new WaitForSeconds(attackObj.startupTime);
        
        // there's probably a better way to handle the below (& its repetitions)
        if (state == EnemyState.Dead || state == EnemyState.Stunned) {
            yield break;
        }

        state = EnemyState.Active;
        // Debug.Log("Active Attack!");
        attackObj.Activate();  // activate attack collider
        attackObj.StartAttack();
        yield return new WaitForSeconds(attackObj.activeTime);

        if (state == EnemyState.Dead || state == EnemyState.Stunned) {
            yield break;
        }

        state = EnemyState.Recovery;
        // Debug.Log("Attack All done");
        attackObj.Deactivate();  // deactivate attack collider
        attackObj.EndAttack();
        yield return new WaitForSeconds(attackObj.recoveryTime);

        if (state == EnemyState.Dead || state == EnemyState.Stunned) {
            yield break;
        }

        state = EnemyState.Passive;
        gameObject.GetComponent<Patrol>().enabled = true;

    }
}
