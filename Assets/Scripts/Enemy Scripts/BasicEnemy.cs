using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using System;
using UnityEngine.AI;


public class BasicEnemy : Enemy
{   
    public float radius = 10.0f;

    public Transform player;
    public NavMeshAgent enemy;

    private int _rotate = 0;
    private float _angles = 0;
    private float _distance;

    // Update is called once per frame
    // in here we should choose an action depending on the enemy's state
    // ex. if passive -> enemy should move randomly around the map
    void Update()
    {  
        // distance per second
        float randMoveSpeed = Random.Range(3, 10);
        _distance = randMoveSpeed * Time.deltaTime;

        //Move to the player
        if (Mathf.Abs(player.position.z - transform.position.z) <= radius)
        {
            state = EnemyState.Tracking;
            enemy.SetDestination(player.position);
        }
        else
        {
            move();
        }
        transform.Translate(0, 0, _distance, Space.Self);
    }

    public override void move() 
    {
        // Precondition: Enemy state is PASSIVE

        if (_rotate < 180)
        {
            _rotate ++;
            transform.Rotate(0, _angles, 0, Space.Self);
        }
        else
        {
            // angle per second
            float randRotateSpeed = Random.Range(-90, 90);
            _angles = randRotateSpeed * Time.deltaTime;
            _rotate = 0;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.tag == "Enemy")
        {   
            Debug.Log("haha get rekt");
            takeHit(100, 2);
        }
    }
}
