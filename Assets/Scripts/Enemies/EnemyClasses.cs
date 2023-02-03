using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Passive,     // 0 - patrolling, "looking" for player
    Tracking,    // 1 - "aggro", trying to get into attack range of player
    Startup,     // 2 - in startup of attack
    Active,      // 3 - attacking, hitbox active
    Recovery,    // 4 - finishing attack
    Stunned,     // 5 - hit by attack, trap, etc. and stunned - cannot move, attack
    Dead         // 6 - rip
}

public abstract class Enemy: MonoBehaviour
{
    [SerializeField]
    int _maxHealth = 100;
    [SerializeField]
    Attack _basicAttack;

    protected int maxHealth { get; set; }
    protected int health { get; set; }
    protected int anger { get; set; }
    protected int[] angerLevels { get; set; }
    protected EnemyState state;
    protected Attack basicAttack { get; set; }

    protected GameObject player;

    // for vision cone
    protected virtual int viewRadius { get; set; }
    protected virtual int viewAngle { get; set; }

    // for debugging
    protected void GetEnemyStatus(string name = "Enemy")
    {
        Debug.Log(System.String.Format("{0}(health: {1}/{2}, anger: {3}, state: {4})", name, health.ToString(), maxHealth.ToString(), anger.ToString(), state));
    }

    // take hitDamage amount of damage and stun for stunTime seconds
    public virtual void TakeHit(int damage, float stunTime)
    {
        health -= damage;
        if (health <= 0)
        {
            StartCoroutine(Die());
        }
        else
        {
            StartCoroutine(GetStunned(stunTime));
        }
    }

    public virtual IEnumerator GetStunned(float stunTime)
    {
        // add animation change for entering hitstun here - turns red for now
        Color originalColor = GetComponent<MeshRenderer>().material.color;
        GetComponent<MeshRenderer>().material.color = Color.red;
        state = EnemyState.Stunned;
        yield return new WaitForSeconds(stunTime);  // waits for stunTime seconds before continuing
        // add animation change for entering patrol/passive state here - returns to original color for now
        GetComponent<MeshRenderer>().material.color = originalColor;
        state = EnemyState.Passive;
    }

    public virtual IEnumerator Die()
    {
        GetComponent<MeshRenderer>().material.color = Color.black;
        state = EnemyState.Dead;
        yield return new WaitForSeconds(5);  // waits for 1 second before destroying object
        Destroy(gameObject);
    }

    public virtual void GetTaunted(int tauntValue)
    {
        anger = anger + tauntValue;
    }

    protected IEnumerator Attack(Attack attackObj) {
        state = EnemyState.Startup;
        // do startup animation
        yield return new WaitForSeconds(attackObj.startupTime);

        state = EnemyState.Active;
        // do active attacking animation
        attackObj.Activate();  // activate attack collider
        yield return new WaitForSeconds(attackObj.activeTime);

        state = EnemyState.Recovery;
        // do cooldown animation
        attackObj.Deactivate();  // deactivate attack collider
        yield return new WaitForSeconds(attackObj.recoveryTime);
        
        state = EnemyState.Passive;
    }

    protected virtual void Start()
    {
        maxHealth = _maxHealth;
        health = _maxHealth;
        anger = 0;
        state = EnemyState.Passive;
        basicAttack = _basicAttack;
        player = GameObject.FindWithTag("Player");
    }
}