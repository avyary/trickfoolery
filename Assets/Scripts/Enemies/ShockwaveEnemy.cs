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

                break;
            
            case EnemyState.Active:
                break;
        }
    }
}
