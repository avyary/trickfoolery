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
        if (!gameManager.isPaused)
        {
            switch(state)
            {
                case EnemyState.Passive:
                    TestBehaviors.Rotate(gameObject, moveSpeed);  // replace with better movement
                    if (!fow.active)
                    {
                        fow.active = true;
                        StartCoroutine(fow.FindPlayer(moveSpeed, PlayerFound));
                    }
                    break;
                case EnemyState.Tracking:
                    TestBehaviors.MoveToPlayer(gameObject, player, moveSpeed);
                    if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= currentAttack.range)
                    {
                        StartCoroutine(Attack(currentAttack));
                    }
                    break;
                case EnemyState.Active:
                    TestBehaviors.MoveForward(gameObject, chargeSpeed);
                    break;
            }
        }
    }
}