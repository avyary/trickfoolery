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


    private Vector3 teleport_direction;
    private float current_teleport_strength;
    private float attackcd;

    protected override void Start() {
        base.Start();
        GetEnemyStatus("TauntEnemy");
        teleporting = false;
        boundary_fov = GameObject.Find("TauntEnemyBoundary").GetComponent<FieldOfView>();
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
        yield return new WaitForSeconds(0.5f);
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
                    Debug.Log("Teleporting Towards Player");
                    StartCoroutine(Teleport(tracking_teleport_strength));
                } else if (dist < tracking_distance)
                {
                    teleport_direction = (transform.position - player.transform.position);
                    Debug.Log("Teleporting Away from Player");
                    StartCoroutine(Teleport(tracking_teleport_strength));
                }

                //StartCoroutine(AttackDelay());
                int chanceToAttack = Random.Range(1, 100);

                if (Input.GetButtonDown("Taunt") && (attack_cooldown <= 0))
                {
                    state = EnemyState.Startup;
                }

                if (boundary_fov.FindVisibleTargets().Count == 0)
                {
                    
                }
                break;
    
            
            case EnemyState.Startup:
                teleport_direction = -1 * (transform.position - player.transform.position);
                StartCoroutine(Teleport(attacking_teleport_strength));
                state = EnemyState.Active;
                break;
            case EnemyState.Active:
                
                
                break;
        }
    }

    public void FixedUpdate()
    {
        if (teleporting)
        {
            transform.Translate(teleport_direction.normalized * current_teleport_strength);
        }
    }
}
