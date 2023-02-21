using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{   
    public Transform player;
    public float radius;

    public NavMeshAgent enemy;
    

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {   

        enemy.SetDestination(player.position);
    
    }
}
