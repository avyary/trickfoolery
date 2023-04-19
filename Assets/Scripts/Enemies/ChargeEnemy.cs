using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(toPlayer), rotateSpeed * Time.deltaTime);
                // transform.LookAt(player.transform.position);
                break;
            case EnemyState.Active:
                TestBehaviors.MoveForward(gameObject, chargeSpeed);
                break;
        }
    }

    private IEnumerator WaitForSecondsAndStopParticles(float seconds, ParticleSystem particles) 
    {
        yield return new WaitForSeconds(seconds);
        particles.Stop();
    }

    private IEnumerator WaitForSecondsAndPlayParticles(float seconds, ParticleSystem particles) 
    {
        yield return new WaitForSeconds(seconds);
        particles.Play();
    } 

    private IEnumerator WaitForSecondsAndStopRunningAnim(float seconds) 
    {
        yield return new WaitForSeconds(seconds);
        isWalking = true;
        isCharging = false;
    }   
}

