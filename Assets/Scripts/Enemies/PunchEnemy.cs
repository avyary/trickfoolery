using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchEnemy : Enemy
{
    protected override void Start() {
        base.Start();
        GetEnemyStatus("PunchEnemy");
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case EnemyState.Passive:
                // TestBehaviors.Rotate(gameObject, moveSpeed);  // replace with better movement
                // Patrol();
                if (!fow.active)
                {
                    fow.active = true;
                    StartCoroutine(fow.FindPlayer(moveSpeed, PlayerFound));
                }
                break;
            case EnemyState.Tracking:
                // TestBehaviors.MoveToPlayer(gameObject, player, moveSpeed);  // replace with pathing to player
                agent.SetDestination(player.transform.position);
                if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= basicAttack.range)
                {
                    StartCoroutine(Attack(basicAttack));
                }
                break;
            case EnemyState.Active:
                TestBehaviors.MoveForward(gameObject, 5f);
                break;
        }
    }
}
