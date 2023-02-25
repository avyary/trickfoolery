using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TauntState
{
 defensive, 
 active
}
public class TauntEnemy : Enemy
{
    private float _teleportCooldown = 10.0f;
    private FieldOfView passiveFov;
    private FieldOfView activeFov;
    private GameObject _player;
    

    protected override void Start() {
        base.Start();
        GetEnemyStatus("TauntEnemy");
        passiveFov = GameObject.Find("Passive Fov").GetComponent<FieldOfView>();
        activeFov = GameObject.Find("Active Foc").GetComponent<FieldOfView>();
        _player = GameObject.FindWithTag("Player");
    }

    protected void cooldown() 
    {
    }

    protected void TeleportAndAttack() 
    {
        // whoosh
        // rajvi
    }

    protected void Defensive() 
    {
        
    }


    protected void MovePassive() 
    {
        // regular patrol goes here.
    }




    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
    
            case EnemyState.Passive:
                // Checking where player is with its massive passive Fov. If found, switch states
                MovePassive();
                StartCoroutine(passiveFov.FindPlayer(moveSpeed, PlayerFound));
                break;

            case EnemyState.Tracking:
                // Check if activefov is within range of player. 
                if (!activeFov.PlayerIsVisible())
                {
                    //move to player fov.
                    
                }
                
                
                TestBehaviors.MoveToPlayer(gameObject, player, moveSpeed);  // replace with pathing to player
                if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= currentAttack.range)
                {
                    StartCoroutine(Attack(currentAttack));
                }
                break;

            case EnemyState.Active:
    
                // Teleport to player position and attack. Use regular punch attack for now (with very fast
                // paramters). 
                break;

        }
        
        
    }
}
