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

        // basicAttack.startupTime = basicAttack.startupTime * 2;
        // angyAttack.startupTime = angyAttack.startupTime * 2;
    }

    private void OnDrawGizmos() 
    {
       DrawSphere();
    }

    void Update() 
    {   
        Debug.Log(System.String.Format("State: {0}", state));
        switch(state)
        {
            case EnemyState.Passive:
                // if (agent.isStopped) 
                // {
                //     //if stopped disable isWalking to switch to idle
                //     animator.SetBool("isWalking", false); 
                //     isWalking = false;
                //     agent.isStopped = false;
                // }


                if (!fow.active)
                { //if moving enable  isWalking to switch to idle
                 
                    
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
                    StartCoroutine(Attack(currentAttack));
                }

                // if (dist <= currentAttack.range) 
                // {   
                //     // Play the particle system
                //     particleSystem.Play();

                //     // StopEnemy();
                //     animator.SetBool("isWalking", false); 
                //     isWalking = false;
                //     animator.SetBool("isCharging", true);
                //     isCharging = true;
                //     // StartCoroutine(Attack(currentAttack));
                     
                    
                //     isCharging = true;
                //     StartCoroutine(WaitForSecondsAndPlayParticles(0.5f, BackParticleSystem));
                    
                //     // Stop the particle system
                //     particleSystem.Stop();
                //     StartCoroutine(WaitForSecondsAndStopParticles(1.0f, BackParticleSystem));
                //     StartCoroutine(WaitForSecondsAndStopRunningAnim(1.0f));
                // }
    
                break;
            case EnemyState.Active:

                // //play charging anim
                // if (isCharging) 
                // { 
                //     animator.SetBool("isWalking", false);
                //     animator.SetBool("isCharging", true);
                // }
                // else
                // {
                //     animator.SetBool("isWalking", true);
                //     animator.SetBool("isCharging", false);
                // }

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

