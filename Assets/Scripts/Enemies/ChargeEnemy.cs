using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeEnemy : Enemy
{
    [SerializeField] 
    private ParticleSystem particleSystem;
    [SerializeField] 
    private ParticleSystem BackParticleSystem;
    [SerializeField] 
    private float chargeSpeed;

    protected override void Start() {
        base.Start();
        GetEnemyStatus("ChargeEnemy");
    }

    void Update() {
        switch(state)
        {
            case EnemyState.Passive:
                TestBehaviors.Rotate(gameObject, moveSpeed);  // replace with better movement
                if (!fow.active)
                {
                    fow.active = true;
                    StartCoroutine(fow.FindPlayer(moveSpeed, PlayerFound));
                }
                break;
            case EnemyState.Tracking:
                TestBehaviors.MoveToPlayer(gameObject, player, moveSpeed);
                if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= basicAttack.range)
                {
                    StartCoroutine(Attack(basicAttack));
                }
                break;
            case EnemyState.Active:
                TestBehaviors.MoveForward(gameObject, chargeSpeed);
                break;
        }
    }

    // void Update() {
    //     switch(state)
    //     {
    //         case EnemyState.Passive:
    //             // TestBehaviors.Rotate(gameObject, 3);
    //             if (!fow.active)
    //             {
    //                 fow.active = true;
    //                 StartCoroutine(fow.FindPlayer(moveSpeed, PlayerFound));
    //             }
    //             break;
    //         case EnemyState.Tracking:
    //             float dist = Vector3.Distance(gameObject.transform.position, player.transform.position);
    //             Debug.Log(System.String.Format("You are {0} away and I am still following you.", dist));

    //             if (dist <= currentAttack.range) 
    //             {
    //                 // // Play the particle system
    //                 // particleSystem.Play();

    //                 StartCoroutine(Attack(currentAttack));
    //                 // StartCoroutine(WaitForSecondsAndPlayParticles(0.5f, BackParticleSystem));
    //                 // Stop the particle system
    //                 // particleSystem.Stop();
                

    //                 // StartCoroutine(WaitForSecondsAndStopParticles(1.0f, BackParticleSystem));

    //             }
    //             else 
    //             {  
    //                 // GoToTarget();
    //                 TestBehaviors.MoveToPlayer(gameObject, player, 5);
    //             }

    //             state = EnemyState.Passive;
    //             break;
    //         case EnemyState.Active:
    //             TestBehaviors.MoveForward(gameObject, chargeSpeed);
    //             break;
    //     }
    // }   
    
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
    
}

