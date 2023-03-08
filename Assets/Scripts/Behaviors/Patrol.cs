using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour {

    protected NavMeshAgent agent;

    [SerializeField]
    protected float range; //radius of sphere

    protected Transform centrePoint;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        centrePoint = agent.transform;
    }

    
    void Update()
    {
        if(agent.remainingDistance <= agent.stoppingDistance) //done with path
        {   
            // randomly generate a new point to move to
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point)) //pass in our centre point and radius of area
            {
                agent.SetDestination(point);
            }
        }

    }
    
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        { 
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    
}
