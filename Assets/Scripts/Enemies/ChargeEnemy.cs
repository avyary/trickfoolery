using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeEnemy : Enemy
{
    [SerializeField]
    private float chargeSpeed;

    protected override void Start() {
        base.Start();
        GetEnemyStatus("ChargeEnemy");
    }

    void Update() {
        switch(state)
        {
            case EnemyState.Passive:
                // TestBehaviors.Rotate(gameObject, moveSpeed);  // replace with better movement
                if (agent.isStopped) 
                {
                    agent.isStopped = false;
                }
                Patrol();
                if (!fow.active)
                {
                    fow.active = true;
                    StartCoroutine(fow.FindPlayer(moveSpeed, PlayerFound));
                }
                break;
            case EnemyState.Tracking:
                // TestBehaviors.MoveToPlayer(gameObject, player, moveSpeed);
                float dist = Vector3.Distance(gameObject.transform.position, player.transform.position);
                Debug.Log(System.String.Format("You are {0} away and I am still following you.", dist));

                if (dist <= currentAttack.range) 
                {
                    StopEnemy();
                    StartCoroutine(Attack(currentAttack));
                }
                else 
                {
                    GoToTarget();
                }

                state = EnemyState.Passive;
                break;
            case EnemyState.Active:
                TestBehaviors.MoveForward(gameObject, chargeSpeed);
                break;
        }
    }
}