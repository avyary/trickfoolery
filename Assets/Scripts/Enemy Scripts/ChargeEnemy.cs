using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeEnemy : Enemy
{
    protected override void Start() {
        maxHealth = 100;
        base.Start();
        getEnemyStatus();
    }

    // for debugging
    protected override void getEnemyStatus() {
        Debug.Log("ChargeEnemy(health: " + health.ToString() + "/" + maxHealth.ToString() + ", anger: " + anger.ToString() + ", state: " + state + ")");
    }

    // placeholder for testing; rotate the enemy
    void rotatePatrol() {
        gameObject.transform.Rotate(0.0f, .5f, 0.0f);
    }

    // placeholder for testing; move the enemy forward
    void moveForward(int speed) {
        gameObject.transform.position += transform.forward * Time.deltaTime * speed;
    }

    IEnumerator attack(Attack attackObj) {
        state = EnemyState.Startup;
        // do startup animation
        yield return new WaitForSeconds(attackObj.startupTime);
        state = EnemyState.Active;
        // do active attacking animation, movement, etc.
        attackObj.Activate();  // activate attack collider
        yield return new WaitForSeconds(attackObj.activeTime);
        state = EnemyState.Recovery;
        // do cooldown animation
        attackObj.Deactivate();  // deactivate attack collider
        yield return new WaitForSeconds(attackObj.recoveryTime);
        state = EnemyState.Passive;
    }

    void Update() {
        // for testing
        if (Input.GetKeyDown(KeyCode.S))
        {
            getEnemyStatus();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            takeHit(80, 3);
        }
        if (Input.GetKeyDown(KeyCode.W)) // replace with state == EnemyState.Tracking && check for player in attack range
        {
            StartCoroutine(attack(basicAttack));
        }
        if (Input.GetKeyDown(KeyCode.Space))
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
                rotatePatrol();  // replace with better movement
                break;
            case EnemyState.Tracking:
                moveForward(1);  // replace with pathing to player
                break;
            case EnemyState.Active:
                moveForward(5);
                break;
        }
    }

    public override void move(int direction) {
        direction = 0;
    }
}