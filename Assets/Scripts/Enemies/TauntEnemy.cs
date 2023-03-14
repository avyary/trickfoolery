using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
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


    public Vector3 teleport_direction;
    private float current_teleport_strength;
    public float attackcd;
    private int timesTeleportCalled = 0;
    private int timesAttackCalled = 0;
    private int timesTeleportAttackCalled = 0;
    private bool attacking = false;

    protected override void Start() {
        base.Start();
        GetEnemyStatus("TauntEnemy");
        teleporting = false;
        attackcd = attack_cooldown;
    }

    protected void cooldown() 
    {
        // rajvi
    }

    protected void TeleportAndAttack() 
    {
        // whoosh
        // rajvi
    }
    
    IEnumerator Teleport(float strength)
    {
        current_teleport_strength = strength;
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

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(3f);
    }

    protected void Defensive() 
    {
        // when enemies come near it and it's not angy
        // it should pull up its shield and enemies should bounch off of it
        // isaac
    }


    protected void MovePassive() 
    {
        // rotate
        // until cooldown
        // then "teleport"
        // rajvi
    }




    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case EnemyState.Passive:
                //if (agent.isStopped) 
                //{
                    //agent.isStopped = false;
                //}
                //Patrol();

                if (!fow.active)
                {
                    fow.active = true;
                    StartCoroutine(fow.FindPlayer(moveSpeed, PlayerFound));
                }
                break;
            case EnemyState.Tracking:
                if (attackcd > 0)
                    attackcd -= Time.deltaTime;
                dist = Vector3.Distance(gameObject.transform.position, player.transform.position);

                var lookPos = player.transform.position- transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10);
                
                if (dist > tracking_distance + tracking_range)
                {
                    teleport_direction = -1 * (transform.position - player.transform.position);
                    StartCoroutine(Teleport(tracking_teleport_strength));
                } else if (dist < tracking_distance)
                {
                    teleport_direction = (transform.position - player.transform.position);
                    StartCoroutine(Teleport(tracking_teleport_strength));
                }
                

                if ((attackcd <= 0)) //For now, attacks are set. TODO: Chance this to random attacks.
                {
                    state = EnemyState.Startup;
                    StartCoroutine(TeleportAttack());
                }
                break;
            
            case EnemyState.Active:
                break;
        }
    }

    IEnumerator TeleportAttack()
    {
        teleport_direction = -1 * (transform.position - player.transform.position);
        yield return StartCoroutine(Teleport(attacking_teleport_strength));
        yield return StartCoroutine(Attack(currentAttack));
        attackcd = attack_cooldown;
        yield return new WaitForSeconds(0.2f);
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
}
