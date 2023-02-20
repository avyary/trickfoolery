using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeEnemy : Enemy
{
    [SerializeField]
    private float chargeSpeed;

    protected override void Start() {
        base.Start();
        agent.stoppingDistance = currentAttack.range;
        GetEnemyStatus("ChargeEnemy");
    }

    void Update() {
        switch(state)
        {
            case EnemyState.Passive:
                // TestBehaviors.Rotate(gameObject, moveSpeed);  // replace with better movement. random movement function should occur here
                Debug.Log("Enemy is passive.");
                MoveRandom();
                if (!fow.active)
                {
                    fow.active = true;
                    StartCoroutine(fow.FindPlayer(moveSpeed, PlayerFound));
                }
                break;
            case EnemyState.Tracking:
                Debug.Log("Enemy is tracking.");
                TestBehaviors.MoveToPlayer(gameObject, player, moveSpeed);
                if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= currentAttack.range)
                {   
                    Debug.Log("starting coroutine for attack.");
                    StartCoroutine(Attack(currentAttack));
                }
                break;
            case EnemyState.Active:
                Debug.Log("Enemy is active.");
                TestBehaviors.MoveForward(gameObject, chargeSpeed);
                break;
        }
    }
}