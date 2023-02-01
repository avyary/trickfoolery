using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Passive,     // 0
    Tracking,    // 1
    Startup,     // 2
    Active,      // 3
    Recovery,    // 4
    Stunned,     // 5
    Dead         // 6
}

public abstract class Enemy: MonoBehaviour
{
    protected virtual int maxHealth { get; set; }
    protected virtual int health { get; set; }
    protected virtual int anger { get; set; }
    protected virtual int[] angerLevels { get; set; }
    protected EnemyState state;
    public Attack basicAttack;

    // for vision cone
    protected virtual int viewRadius { get; set; }
    protected virtual int viewAngle { get; set; }

    // for debugging
    protected virtual void getEnemyStatus()
    {
        Debug.Log("Enemy(health: " + health.ToString() + "/" + maxHealth.ToString() + ", anger: " + anger.ToString() + ", state: " + state + ")");
    }

    // call in attack upon collision
    public virtual void takeHit(int hitDamage, float stunTime)
    {
        health -= hitDamage;
        if (health <= 0)
        {
            StartCoroutine(die());
            return;
        }
        StartCoroutine(hitStun(stunTime));
    }

    public virtual IEnumerator hitStun(float stunTime)
    {
        // add animation change for entering hitstun here
        Color originalColor = GetComponent<MeshRenderer>().material.color;
        GetComponent<MeshRenderer>().material.color = Color.red;
        state = EnemyState.Stunned;
        yield return new WaitForSeconds(stunTime);  // waits for stunTime seconds before continuing to next line

        // add animation change for entering patrol/passive state here
        GetComponent<MeshRenderer>().material.color = originalColor;
        state = EnemyState.Passive;
    }

    public virtual IEnumerator die()
    {
        GetComponent<MeshRenderer>().material.color = Color.black;
        state = EnemyState.Dead;
        yield return new WaitForSeconds(1);  // waits for 1 second before destroying object
        Destroy(gameObject);
    }

    public virtual void getTaunted(int tauntValue)
    {
        anger = anger + tauntValue;
    }

    protected virtual void Start()
    {
        health = maxHealth;
        anger = 0;
        state = EnemyState.Passive;
    }
}