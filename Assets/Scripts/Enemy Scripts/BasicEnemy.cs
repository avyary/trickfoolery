using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BasicEnemy : Enemy
{   
    public NavMeshAgent enemy;
    public Transform player;
    public float range; //radius of sphere
    protected float radius = 15.0f;   // radius for player detection

    public Transform centrePoint; //centre of the area the agent wants to move around in

    private bool _isWaiting = false;

    protected override void Start()
    {   
        base.Start();
        enemy = GetComponent<NavMeshAgent>();
    }

    void Update()
    {   
        if (state == EnemyState.Passive && !_isWaiting) 
        {
            // move randomly
            Debug.Log("enemy is passive");
            move();

            if (Mathf.Abs(player.position.z - transform.position.z) <= radius) 
            {
                state = EnemyState.Tracking;
                Debug.Log("enemy changed from passive to tracking");
                _pause();
            }
        }

        if (state == EnemyState.Tracking) 
        {   
            Debug.Log("enemy is tracking");
            enemy.SetDestination(player.position);
        }    

    }

    private IEnumerator _pause() 
    {   
        _isWaiting = true;
        yield return new WaitForSeconds(10);
        _isWaiting = false;
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        { 
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            // or add a for loop
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    public override void move() 
    {
        if(enemy.remainingDistance <= enemy.stoppingDistance) //done with path
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point)) //pass in our centre point and radius of area
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                enemy.SetDestination(point);
            }
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
