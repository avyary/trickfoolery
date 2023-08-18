using System.Collections;
using UnityEngine;

//*******************************************************************************************
// ShockwaveEnemy
//*******************************************************************************************
/// <summary>
/// Enemy subclass that further specifies ShockwaveEnemy type behaviors such as
/// patrolling the map and initiating shockwave attacks when sighting the player.
/// </summary>
public class ShockwaveEnemy : Enemy
{
    [SerializeField]
    private AudioClip attackSound;

    [SerializeField]
    private float rotateSpeed;

    /// <summary>
    /// Extends the parent class initialization of bookkeeping structures with debugging functionality to log
    /// this Enemy's data.
    /// </summary>
    protected override void Start()
    {
        base.Start();
        
        GetEnemyStatus("ShockwaveEnemy");
    }

    void Update()
    {
        switch (state)
        {
            case EnemyState.Patrolling:
                if (!fow.active)    
                {
                    fow.active = true;
                }
                StartCoroutine(fow.FindPlayer(moveSpeed, PlayerFound));
                break;
            
            case EnemyState.Tracking:
                agent.SetDestination(player.transform.position);
                float dist = Vector3.Distance(gameObject.transform.position, player.transform.position);
                if (dist <= basicAttack.range)
                {
                    print("in range");
                    StartCoroutine(Attack(currentAttack));
                    agent.ResetPath();
                    state = EnemyState.Startup;
                }
                break;
            case EnemyState.Startup:
                Vector3 toPlayer = player.transform.position - transform.position;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(toPlayer), rotateSpeed * Time.deltaTime);
                break;
        }
    }
    
    /// <summary>
    /// Sets this GameObject's position to its current position and waits for a duration of time.
    /// </summary>
    /// <param name="sec"> The duration of time to wait after resetting this GameObject's position in seconds. </param>
    IEnumerator Freeze(float sec) {
        // Save the object's current position
        Vector3 originalPosition = transform.position;

        // Set the object's position to its current position, effectively "freezing" it
        transform.position = originalPosition;

        // Wait for 3 seconds
        yield return new WaitForSeconds(sec);
    }
}
