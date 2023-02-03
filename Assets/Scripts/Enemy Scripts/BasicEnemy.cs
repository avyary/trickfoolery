using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BasicEnemy : Enemy
{   
    public NavMeshAgent agent;
    public float range; //radius of sphere

    public Transform centrePoint; //centre of the area the agent wants to move around in

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(agent.remainingDistance <= agent.stoppingDistance) //done with path
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point)) //pass in our centre point and radius of area
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                agent.SetDestination(point);
            }
        }

    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        { 
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    public override void move() 
    {
        ;
    }



    // public float radius = 10.0f;

    // public Transform player;
    // public NavMeshAgent enemy;

    // private int _rotate = 0;
    // private float _angles = 0;
    // private float _distance;

    // // Update is called once per frame
    // // in here we should choose an action depending on the enemy's state
    // // ex. if passive -> enemy should move randomly around the map
    // void Update()
    // {  
    //     // distance per second
    //     float randMoveSpeed = Random.Range(3, 10);
    //     _distance = randMoveSpeed * Time.deltaTime;

    //     //Move to the player
    //     if (Mathf.Abs(player.position.z - transform.position.z) <= radius)
    //     {
    //         state = EnemyState.Tracking;
    //         enemy.SetDestination(player.position);
    //     }
    //     else
    //     {   
    //         _moveTowardsPlayer();
    //     }
    //     transform.Translate(0, 0, _distance, Space.Self);
    // }

    // private IEnumerator _moveTowardsPlayer() 
    // {
    //     yield return new WaitForSeconds(10);
    //     move();
    // }

    // public override void move() 
    // {
    //     // Precondition: Enemy state is PASSIVE

    //     if (_rotate < 180)
    //     {
    //         _rotate ++;
    //         transform.Rotate(0, _angles, 0, Space.Self);
    //     }
    //     else
    //     {
    //         // angle per second
    //         float randRotateSpeed = Random.Range(-90, 90);
    //         _angles = randRotateSpeed * Time.deltaTime;
    //         _rotate = 0;
    //     }
    // }

    // private void OnCollisionEnter(Collision collision) {
    //     if (collision.collider.tag == "Enemy")
    //     {   
    //         Debug.Log("haha get rekt");
    //         takeHit(100, 2);
    //     }
    // }
}
