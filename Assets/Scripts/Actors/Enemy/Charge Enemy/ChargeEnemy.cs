using System.Collections;
using UnityEngine;

//*******************************************************************************************
// ChargeEnemy
//*******************************************************************************************
/// <summary>
/// Enemy subclass that further specifies ChargeEnemy type behaviors such as patrolling
/// the map and initiating charge attacks when sighting the player. Also handles particle
/// effect systems associated with charge attacks.
/// </summary>
public class ChargeEnemy : Enemy
{
    [SerializeField]
    private float chargeSpeed;
    public AK.Wwise.Event chargerDeathSFX;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private ParticleSystem BackParticleSystem;
    public bool isCharging;
    public bool isWalking;

    [SerializeField]
    private float rotateSpeed;

    /// <summary>
    /// Extends the parent class initialization of bookkeeping structures with debugging functionality to log
    /// this Enemy's data and changing of states.
    /// </summary>
    protected override void Start() {
        base.Start();
        GetEnemyStatus("ChargeEnemy");

        isWalking = true;
    }

    void Update() 
    {   
        switch(state)
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
                if (dist <= currentAttack.range) 
                {   
                    attackCoroutine = StartCoroutine(Attack(currentAttack));
                    agent.ResetPath();
                
                    particleSystem.Play();

                    StartCoroutine(WaitForSecondsAndPlayParticles(0.5f, BackParticleSystem));

                    // Stop the particle system
                    particleSystem.Stop();
                    StartCoroutine(WaitForSecondsAndStopParticles(1.0f, BackParticleSystem));
                    StartCoroutine(WaitForSecondsAndStopRunningAnim(1.0f));
                }
    
                break;
            case EnemyState.Startup:
                Vector3 toPlayer = player.transform.position - transform.position;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, 
                    Quaternion.LookRotation(toPlayer), rotateSpeed * Time.deltaTime);
                // transform.LookAt(player.transform.position);
                break;
            case EnemyState.Active:
                TestBehaviors.MoveForward(gameObject, chargeSpeed);
                break;
        }
    }

    /// <summary>
    /// Delays for a duration of time before disabling the ParticleSystem.
    /// </summary>
    /// <param name="seconds"> The duration of time to wait before disabling the particle system in seconds. </param>
    /// <param name="particles"> The ParticleSystem to be disabled. </param>
    private IEnumerator WaitForSecondsAndStopParticles(float seconds, ParticleSystem particles) 
    {
        yield return new WaitForSeconds(seconds);
        particles.Stop();
    }

    /// <summary>
    /// Delays for a duration of time before enabling the ParticleSystem.
    /// </summary>
    /// <param name="seconds"> The duration of time to wait before enabling the particle system in seconds. </param>
    /// <param name="particles"> The ParticleSystem to be enabled. </param>
    private IEnumerator WaitForSecondsAndPlayParticles(float seconds, ParticleSystem particles) 
    {
        yield return new WaitForSeconds(seconds);
        particles.Play();
    } 

    /// <summary>
    /// Delays for the duration of "seconds" before setting the Enemy flags from charging to walking.
    /// </summary>
    /// <param name="seconds"> The duration of time to wait before setting flags in seconds. </param>
    private IEnumerator WaitForSecondsAndStopRunningAnim(float seconds) 
    {
        yield return new WaitForSeconds(seconds);
        isWalking = true;
        isCharging = false;
    }   
}

