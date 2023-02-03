using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntEnemy : Enemy
{
    protected override void Start() {
        base.Start();
        GetEnemyStatus("TauntEnemy");
    }

    // Update is called once per frame
    void Update()
    {
        // for testing
        if (Input.GetKeyDown(KeyCode.A))
        {
            TakeHit(80, 1);
        }
        if (Input.GetKeyDown(KeyCode.S)) // replace with state == EnemyState.Tracking && check for player in attack range
        {
            StartCoroutine(Attack(basicAttack));
        }
        if (Input.GetKeyDown(KeyCode.D))
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
        }
    }
}
