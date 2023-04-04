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
    public Animator animator;
    public bool isCharging;
    public bool isWalking;

    protected override void Start() {
        base.Start();
        GetEnemyStatus("ChargeEnemy");

        animator.SetBool("isWalking", true); 
        isWalking = true;
    }

    void Update() 
    {   
        switch(state)
        {
            case EnemyState.Passive:
                break;

            case EnemyState.Patrolling:
                if (!fow.active)
                { 
                    fow.active = true;
                    StartCoroutine(fow.FindPlayer(moveSpeed, PlayerFound));
                }
                break;
            case EnemyState.Tracking:

                agent.SetDestination(player.transform.position);
                float dist = Vector3.Distance(gameObject.transform.position, player.transform.position);

                if (dist <= currentAttack.range) 
                {   
                    agent.ResetPath();

                    particleSystem.Play();
                    animator.SetBool("isWalking", false);
                    isWalking = false;
                    animator.SetBool("isCharging", true);
                    isCharging = true;
                    StartCoroutine(Attack(currentAttack));

                    StartCoroutine(WaitForSecondsAndPlayParticles(0.5f, BackParticleSystem));

                    // Stop the particle system
                    particleSystem.Stop();
                    StartCoroutine(WaitForSecondsAndStopParticles(1.0f, BackParticleSystem));
                    StartCoroutine(WaitForSecondsAndStopRunningAnim(1.0f));
                }
    
                break;
            case EnemyState.Active:
                //play charging anim
                if (isCharging) 
                { 
                    animator.SetBool("isWalking", false);
                    isCharging = true;
                }
                else
                {
                    animator.SetBool("isWalking", true);
                    animator.SetBool("isCharging", false);
                }

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
        animator.SetBool("isWalking", true);
        animator.SetBool("isCharging", false);
        isWalking = true;
        isCharging = false;
    }   
}

