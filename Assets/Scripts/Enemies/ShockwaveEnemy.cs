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
        Debug.Log(System.String.Format("Range: {0}", currentAttack.range));
        ;
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
                    agent.ResetPath();
                    StartCoroutine(Attack(currentAttack));
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
}
