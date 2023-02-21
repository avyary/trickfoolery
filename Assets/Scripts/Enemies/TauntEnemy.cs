using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntEnemy : Enemy
{   
    private float _teleportCooldown = 10.0f;

    protected override void Start() {
        base.Start();
        GetEnemyStatus("TauntEnemy");
    }

    protected void cooldown() 
    {
        // rajvi
    }

    protected void TeleportAndAttack() 
    {
        // whoosh
        // rajvi
    }

    protected void Defensive() 
    {
        // when enemies come near it and it's not angy
        // it should pull up its shield and enemies should bounch off of it
        // isaac
    }


    protected void MovePassive() 
    {
        // rotate
        // until cooldown
        // then "teleport"
        // rajvi
    }




    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
    
            case EnemyState.Passive:
                // TestBehaviors.Rotate(gameObject, moveSpeed);    // replace with better better movement
                // if (!fow.active)
                // {
                //     fow.active = true;
                //     StartCoroutine(fow.FindPlayer(moveSpeed, PlayerFound));
                // }

                MovePassive();

                break;

            case EnemyState.Tracking:
                // if there is a straight line of sight between the enemy and the player, and cooldown is up, run fast to the player's position
                // switch to attack stage

                // TestBehaviors.MoveToPlayer(gameObject, player, moveSpeed);  // replace with pathing to player
                // if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= basicAttack.range)
                // {
                //     StartCoroutine(Attack(basicAttack));    // we need to set the Taunting Enemy's basic attack
                // }

                TeleportAndAttack();
                state = EnemyState.Passive();
                break;

            case EnemyState.Active:
    
                // TestBehaviors.MoveForward(gameObject, 5f);
                break;

        }
    }
}
