using UnityEngine;
using UnityEngine.AI;

//*******************************************************************************************
// EnemyMovement
//*******************************************************************************************
/// <summary>
/// Handles the generation of enemy movement paths through the NavMeshAgent.
/// </summary>
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
