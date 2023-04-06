using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public enum EnemyState
{
    Passive,     // 0 - patrolling, "looking" for player
    Tracking,    // 1 - "aggro", trying to get into attack range of player
    Startup,     // 2 - in startup of attack
    Active,      // 3 - attacking, hitbox active
    Recovery,    // 4 - finishing attack
    Stunned,     // 5 - hit by attack, trap, etc. and stunned - cannot move, attack
    Dead,        // 6 - rip
    Spawning
}

public abstract class Enemy: MonoBehaviour
{
    [SerializeField] 
    GameObject _AngyInd;
    [SerializeField]
    GameObject _AlertInd;
    [SerializeField]
    int _maxHealth;
    [SerializeField]
    float _moveSpeed;
    [SerializeField]
    int _maxAnger;
    [SerializeField]
    Attack _basicAttack;
    [SerializeField]
    Attack _angyAttack;
    [SerializeField]
    GameObject _deathBubble;

    //wwise
    public AK.Wwise.Event attackSFX;
    public AK.Wwise.Event chargerDeathSFX;
    public AK.Wwise.Event angySFX;

    public bool isAngy;
    public Attack currentAttack;

    protected int maxHealth { get; set; }
    protected Attack basicAttack { get; set; }
    protected Attack angyAttack { get; set; }
    protected float moveSpeed { get; set; }
    protected int maxAnger { get; set; }

    protected int health { get; set; }
    protected int anger { get; set; }
    [SerializeField] protected EnemyState state;

    protected GameObject player;
    protected HypeManager hypeManager;
    protected FieldOfView fow;

    protected AudioSource audioSource;
    protected bool inHitPause;

    protected NavMeshAgent agent;   // this is the enemy
    protected float range = 21.76f;  // radius of sphere for generating a random point
    protected Transform centrePoint;    // centre of the map (try setting it to the agent for fun?)

    protected GameObject deathBubble;


    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        { 
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    // for debugging
    public void GetEnemyStatus(string name = "Enemy")
    {
        Debug.Log(System.String.Format("{0}(health: {1}/{2}, anger: {3}/{4}, state: {5})", name, health.ToString(), maxHealth.ToString(), anger.ToString(), maxAnger.ToString(), state));
    }

    // invoked when colliding with attack hitbox
    public virtual void TakeHit(Enemy attacker, int damage, float stunTime)
    {
        if (state == EnemyState.Dead)
        {
            return;
        }

        health -= damage;
        StartCoroutine(attacker.GetHitPaused(0.5f));
        if (health <= 0)
        {
            StartCoroutine(Die());
        }
        else
        {
            hypeManager.IncreaseHype(hypeManager.HIT_HYPE);
            StartCoroutine(GetHitPaused(0.5f));
            StartCoroutine(GetStunned(stunTime));
        }
    }

    // 
    public virtual IEnumerator GetHitPaused(float pauseTime)
    {
        inHitPause = true;
        yield return new WaitForSeconds(pauseTime);
        inHitPause = false;
    }

    // puts enemy in Stunned state for stunTime seconds
    public virtual IEnumerator GetStunned(float stunTime)
    {
        // replace with animation change for entering hitstun here - turns red for now
        Color originalColor = GetComponent<MeshRenderer>().material.color;
        GetComponent<MeshRenderer>().material.color = Color.red;

        state = EnemyState.Stunned;
        yield return new WaitForSeconds(stunTime);  // waits for stunTime seconds before continuing

        // add animation change for entering patrol/passive state here - returns to original color for now
        GetComponent<MeshRenderer>().material.color = originalColor;

        state = EnemyState.Passive;
    }

    // invoked when health falls to/below 0
    public virtual IEnumerator Die()
    {
        hypeManager.IncreaseHype(hypeManager.DEATH_HYPE);
        //deathBubble.SetActive(true);

        fow.active = false;
        basicAttack.Deactivate();  // deactivate attack collider

        chargerDeathSFX.Post(gameObject);

        state = EnemyState.Dead;
        yield return new WaitForSeconds(0.5f);  // waits before destroying object

        Destroy(gameObject);
    }

    // invoked when player taunts and enemy is in taunt radius
    public virtual void GetTaunted(int tauntValue = 1)
    {
        anger = anger + tauntValue;
        if (anger >= maxAnger) {
            isAngy = true;
            angySFX.Post(gameObject);
            _AngyInd.SetActive(true); 
            currentAttack = angyAttack;
            Debug.Log("Damage " + currentAttack.damage);
        }else{_AngyInd.SetActive(false);}
    }


    public IEnumerator Attack(Attack attackObj) {
        // trigger attack animation here
        state = EnemyState.Startup;
        // Debug.Log("Attacking Time");
        yield return new WaitForSeconds(attackObj.startupTime);
        
        // there's probably a better way to handle the below (& its repetitions)
        if (state == EnemyState.Dead || state == EnemyState.Stunned) {
            yield break;
        }

        state = EnemyState.Active;
        // Debug.Log("Active Attack!");
        attackSFX.Post(gameObject);
        attackObj.Activate();  // activate attack collider
        yield return new WaitForSeconds(attackObj.activeTime);

        if (state == EnemyState.Dead || state == EnemyState.Stunned) {
            yield break;
        }

        state = EnemyState.Recovery;
        // Debug.Log("Attack All done");
        attackObj.Deactivate();  // deactivate attack collider
        yield return new WaitForSeconds(attackObj.recoveryTime);

        if (state == EnemyState.Dead || state == EnemyState.Stunned) {
            yield break;
        }

        state = EnemyState.Passive;
        gameObject.GetComponent<Patrol>().enabled = true;
    }

    protected virtual void PlayerFound()
    {
        // animation for finding player?
        state = EnemyState.Tracking;
        // Debug.Log("Player found!");
        gameObject.GetComponent<Patrol>().enabled = false;
        PlayerDetected();
    }

    protected virtual void PlayerDetected() 
    {
        _AlertInd.SetActive(true);
    }

    // protected virtual void DrawSphere()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, fow.viewRadius);
    // }

    // protected virtual void StopEnemy() 
    // {
    //     agent.isStopped = true;

    // }

    // protected virtual void GoToTarget()
    // {
    //     agent.isStopped = false;
    //     agent.SetDestination(player.transform.position);
    // }

    protected virtual void Move()
    {
        return;
    }

    protected virtual void Start()
    {
        maxHealth = _maxHealth;
        health = _maxHealth;
        maxAnger = _maxAnger;
        StartCoroutine(DelayStart());
        anger = 0;
        moveSpeed = _moveSpeed;
        basicAttack = _basicAttack;
        angyAttack = _angyAttack;
        currentAttack = _basicAttack;
        // deathBubble = _deathBubble;
        // deathBubble.SetActive(false);
        player = GameObject.FindWithTag("Player");
        hypeManager = GameObject.Find("Game Manager").GetComponent<HypeManager>();
        fow = gameObject.GetComponent<FieldOfView>();
        agent = GetComponent<NavMeshAgent>();
        centrePoint = agent.transform;
        audioSource = GetComponent<AudioSource>();
    }

    IEnumerator DelayStart() {
        state = EnemyState.Spawning;
        yield return new WaitForSecondsRealtime(0.5f);
        state = EnemyState.Passive;
    }

}