using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeEnemy : Enemy
{
    [SerializeField] private ParticleSystem particleSystem;
        [SerializeField] private ParticleSystem BackParticleSystem;
    [SerializeField] private float chargeSpeed;

    protected override void Start() {
        base.Start();
        GetEnemyStatus("ChargeEnemy");
    }

    void Update() {
        switch(state)
        {
            case EnemyState.Passive:
                if (agent.isStopped) 
                {
                    agent.isStopped = false;
                }
                Patrol();
                if (!fow.active)
                {
                    fow.active = true;
                    StartCoroutine(fow.FindPlayer(moveSpeed, PlayerFound));
                }
                break;
            case EnemyState.Tracking:
                float dist = Vector3.Distance(gameObject.transform.position, player.transform.position);
                Debug.Log(System.String.Format("You are {0} away and I am still following you.", dist));

                if (dist <= currentAttack.range) 
                {
                    // Play the particle system
                    particleSystem.Play();

                    StopEnemy();

                    StartCoroutine(Attack(currentAttack));
                  StartCoroutine(WaitForSecondsAndPlayParticles(0.5f, BackParticleSystem));
                    // Stop the particle system
                    particleSystem.Stop();
                

                    StartCoroutine(WaitForSecondsAndStopParticles(0.2f, BackParticleSystem));

                }
                else 
                {  
                    GoToTarget();
                }

                state = EnemyState.Passive;
                break;
            case EnemyState.Active:
                TestBehaviors.MoveForward(gameObject, chargeSpeed);
                break;
        }
    }   private IEnumerator WaitForSecondsAndStopParticles(float seconds, ParticleSystem particles) {
        yield return new WaitForSeconds(seconds);
        particles.Stop();
    } 
    private IEnumerator WaitForSecondsAndPlayParticles(float seconds, ParticleSystem particles) {
        yield return new WaitForSeconds(seconds);
        particles.Play();
    } 
    
    }

