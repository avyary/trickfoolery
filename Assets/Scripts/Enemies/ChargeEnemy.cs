using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeEnemy : Enemy
{
    protected override void Start() {
        base.Start();
        GetEnemyStatus("ChargeEnemy");
    }

    // placeholder for testing; move the enemy forward
    void moveForward(int speed) {
        gameObject.transform.position += transform.forward * Time.deltaTime * speed;
    }

    void Update() {
        // for testing
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeHit(80, 3);
        }
        if (Input.GetKeyDown(KeyCode.W)) // replace with state == EnemyState.Tracking && check for player in attack range
        {
            StartCoroutine(Attack(basicAttack));
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (state == EnemyState.Tracking)
            {
                state = EnemyState.Passive;
            }
            else if (state == EnemyState.Passive)
            {
                state = EnemyState.Tracking;
            }
        }

        switch(state)
        {
            case EnemyState.Passive:
                TestBehaviors.Rotate(gameObject, 0.5f);  // replace with better movement
                if (!fow.active)
                {
                    fow.active = true;
                    StartCoroutine(fow.FindPlayer(0.2f, PlayerFound));
                }
                break;
            case EnemyState.Tracking:
                TestBehaviors.MoveToPlayer(gameObject, player, 0.01f);  // replace with pathing to player
                if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= basicAttack.range)
                {
                    StartCoroutine(Attack(basicAttack));
                }
                break;
            case EnemyState.Active:
                TestBehaviors.MoveForward(gameObject, 10f);  // replace with pathing to player
                break;
        }
    }
}