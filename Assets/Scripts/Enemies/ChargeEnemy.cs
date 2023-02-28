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
        Debug.Log(System.String.Format("State: {0}", state));
        switch(state)
        {
            case EnemyState.Passive:
                // TestBehaviors.Rotate(gameObject, moveSpeed);  // replace with better movement
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

                if (dist <= basicAttack.range)
                {
                    StopEnemy();
                    if (isAngy)
                    {
                        StartCoroutine(Attack(angyAttack));
                    }
                    else
                    {
                        StartCoroutine(Attack(basicAttack));   
                    }
                }
                else 
                {
                    GoToTarget();
                }
                break;
            case EnemyState.Active:
                TestBehaviors.MoveForward(gameObject, chargeSpeed);
                break;
        }
    }
}