using System;
using System.Collections;
using System.Collections.Generic;
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
    private float _teleportCooldown = 5.0f;
    [SerializeField] private float tracking_distance = 10.0f;
    [SerializeField] private float tracking_range = 3.0f;
    [SerializeField] private float tracking_teleport_strength = 30.0f;//The distance the taunting enemy will keep between the player. 
    [SerializeField] private float attacking_teleport_strength = 30.0f;//The distance the taunting enemy will cover to get close to the player.
    [SerializeField] private float dist;
    [SerializeField] private float teleport_time = 0.10f;
    [SerializeField] private bool teleporting;
    [SerializeField] private FieldOfView boundary_fov;
    [SerializeField] private float attack_cooldown;


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
        if (attackcd > 0)
            attackcd -= Time.deltaTime;
        
        
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
                dist = Vector3.Distance(gameObject.transform.position, player.transform.position);

                if (dist > tracking_distance + tracking_range)
                {
                    teleport_direction = -1 * (transform.position - player.transform.position);
                    StartCoroutine(Teleport(tracking_teleport_strength));
                } else if (dist < tracking_distance)
                {
                    teleport_direction = (transform.position - player.transform.position);
                    StartCoroutine(Teleport(tracking_teleport_strength));
                }

                //StartCoroutine(AttackDelay());
                int chanceToAttack = Random.Range(1, 1000);

                if ((Input.GetButton("Taunt")) && (attackcd <= 0))
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
        timesTeleportCalled++;
        yield return StartCoroutine(Attack(currentAttack));

        yield return new WaitForSeconds(0.2f);
        state = EnemyState.Tracking;
        yield return null;
    }
    public void FixedUpdate()
    {
        if (teleporting)
        {
            transform.Translate(teleport_direction.normalized * current_teleport_strength);
        }
    }
}
