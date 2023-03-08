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

    public LineRenderer circleRenderer;

    protected override void Start() {
        base.Start();
        GetEnemyStatus("ChargeEnemy");
        Debug.Log(System.String.Format("View Radius: {0}", fow.viewRadius));
        Debug.Log(System.String.Format("View Angle: {0}", fow.viewAngle));
    }

    void Update() {
        Debug.Log(System.String.Format("Current state is: {0}", state));
        switch(state)
        {
            case EnemyState.Passive:
                if (!fow.active)
                {
                    fow.active = true;
                    StartCoroutine(fow.FindPlayer(moveSpeed, PlayerFound));
                }
                break;
            case EnemyState.Tracking:
                // TestBehaviors.MoveToPlayer(gameObject, player, moveSpeed);
                agent.SetDestination(player.transform.position);
                
                if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= basicAttack.range)
                {   
                    agent.ResetPath();
                    StartCoroutine(Attack(basicAttack));
                }
                break;
            case EnemyState.Active:
                TestBehaviors.MoveForward(gameObject, chargeSpeed);
                break;
        }
    }

    void DrawCircle(int steps, float radius) 
    {
        circleRenderer.positionCount = steps;

        for (int currentStep = 0; currentStep < steps; currentStep++)
        {
            float circumferenceProgress = (float) currentStep / steps;
            float currentRadian = circumferenceProgress * 2 * Mathf.PI;
            
            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * radius;
            float y = yScaled * radius;

            Vector3 currentPosition = new Vector3(x, y, 0);

            circleRenderer.SetPosition(currentStep, currentPosition);
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

