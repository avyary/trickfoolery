using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public enum TauntState
{
    passive,
    teleporting
    
}

public class TauntEnemy : Enemy
{   
    private float _teleportCooldown;
    [SerializeField] private float tracking_distance;
    [SerializeField] private float tracking_range;
    [SerializeField] private float tracking_teleport_strength;//The distance the taunting enemy will keep between the player. 
    [SerializeField] private float attacking_teleport_strength;//The distance the taunting enemy will cover to get close to the player.
    [SerializeField] private float dist;
    [SerializeField] private float teleport_time;
    [SerializeField] private bool teleporting;
    [SerializeField] private float attack_cooldown;
    [SerializeField] private ParticleSystem DashParticle;
    [SerializeField] private TauntAnimationController animationController;


    public Vector3 teleport_direction;
    private float current_teleport_strength;
    public float attackcd;
    private int timesTeleportCalled = 0;
    private int timesAttackCalled = 0;
    private int timesTeleportAttackCalled = 0;
    private bool attacking = false;
    private bool isTracking = false;

    //wwise
    public AK.Wwise.Event beefyBoyDashSFX;

    private bool isRunning;
    private bool isAttacking;
    private bool isWalking;
    public bool dancetime;

    protected override void Start() {
        base.Start();
        GetEnemyStatus("TauntEnemy");
        teleporting = false;
        attackcd = attack_cooldown;
        gameObject.GetComponent<Patrol>().enabled = false;
        dancetime = false;
        state = EnemyState.Patrolling;
    }

    IEnumerator Teleport(float strength)
    {
        animator.ResetTrigger("Roll");
        animationController.doneRolling = false;
        current_teleport_strength = strength;
        animator.SetTrigger("Roll");
        beefyBoyDashSFX.Post(gameObject);
        StartCoroutine(WaitForSecondsAndStopTeleportAnim(0.5f));
        float startTime = Time.time;

        while (Time.time < startTime + teleport_time)
        {
            teleporting = true;
            yield return null;
        }
        teleporting = false;
        DashParticle.Play();

        StartCoroutine(WaitForSecondsAndStopParticles(0.2f, DashParticle));
    }
    
    private IEnumerator WaitForSecondsAndStopParticles(float seconds, ParticleSystem particles) {
        yield return new WaitForSeconds(seconds);
        particles.Stop();
    } 
    


    // Update is called once per frame
    void Update()
    {   
        
        gameObject.GetComponent<Patrol>().enabled = false;// ensure this is always false
        switch(state)
        {
            case EnemyState.Patrolling:
                //if (agent.isStopped) 
                //{
                    //agent.isStopped = false;
                //}
                //Patrol();

                if (!gameObject.GetComponent<FieldOfView>().active)
                {
                    gameObject.GetComponent<FieldOfView>().active = true;
                    StartCoroutine(gameObject.GetComponent<FieldOfView>().FindPlayer(moveSpeed, PlayerFound));
                }
                break;
            case EnemyState.Tracking:
              

                    isTracking = true;
                    if (attackcd > 0)
                        attackcd -= Time.deltaTime;
                    dist = Vector3.Distance(gameObject.transform.position, player.transform.position);

                    var lookPos = player.transform.position - transform.position;
                    lookPos.y = 0;
                    var rotation = Quaternion.LookRotation(lookPos);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);

                    if ((dist > tracking_distance + tracking_range) && animationController.doneRolling)
                    {
                        animator.SetBool("dancetime", false);
                        teleport_direction = -1 * (transform.position - player.transform.position);
                        StartCoroutine(Teleport(tracking_teleport_strength));
                    }
                    else if ((dist < tracking_distance)&& animationController.doneRolling)
                    {
                        animator.ResetTrigger("Dance");
                        animator.SetBool("dancetime", false);
                        teleport_direction = (transform.position - player.transform.position);
                        rotation = Quaternion.LookRotation(lookPos);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 50);
                        StartCoroutine(Teleport(tracking_teleport_strength));
                    }
                    


                    if ((attackcd <= 0) && animationController.doneRolling) //For now, attacks are set. TODO: Chance this to random attacks.
                    {
                        state = EnemyState.Startup;
                        StartCoroutine(TeleportAttack());
                    }
                    
                    
                    animator.SetBool("dancetime", true);
                    

                    break;
            
            case EnemyState.Active:
                break;
        }
    }

    IEnumerator TeleportAttack()
    {   //TODO: WWISE SOUNDS FOR ATTACK ANINMATION SHOULD GO HERE 
        animationController.doneAttacking = false;
        Vector3 toPlayer = player.transform.position - transform.position;
        teleport_direction = -1 * (transform.position - player.transform.position);
        //yield return StartCoroutine(Teleport(attacking_teleport_strength));
        yield return StartCoroutine(Teleport(toPlayer.magnitude * 0.06f));
        while (animationController.doneRolling == false)
        {
            yield return null;
        }
        animator.SetTrigger("Attack");
        currentAttack._renderer.material.color = new Color(255, 165, 0, 0.2f);
        currentAttack._renderer.enabled = true;
        while (animationController.doneAttacking == false)
        {
            yield return null;
        }
        currentAttack._renderer.material.color = new Color(255, 0, 0, 0.2f);
        yield return StartCoroutine(Attack(currentAttack));
        attackcd = attack_cooldown;
        animator.ResetTrigger("Attack");
        state = EnemyState.Tracking;
        yield return null;
    }
    public void FixedUpdate()
    {
        if (teleporting)
        {
            transform.Translate(teleport_direction.normalized * current_teleport_strength, Space.World);
        } 
    }
    
    
    private IEnumerator WaitForSecondsAndStopTeleportAnim(float seconds) 
    {
        yield return new WaitForSeconds(seconds);
    }   
}
