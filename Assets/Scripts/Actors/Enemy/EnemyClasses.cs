using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;


public enum EnemyState
{
    Passive,     // 0 - out of combat;
    Patrolling,  // 1 - "looking" for player
    Tracking,    // 2 - "aggro", trying to get into attack range of player
    Startup,     // 3 - in startup of attack
    Active,      // 4 - attacking, hitbox active
    Recovery,    // 5 - finishing attack
    Stunned,     // 6 - hit by attack, trap, etc. and stunned - cannot move, attack
    Dead,        // 7 - rip
    Spawning     // 8
}

//*******************************************************************************************
// Enemy
//*******************************************************************************************
/// <summary>
/// Abstract class that defines basic states for enemy classes, handling taking damage,
/// getting stunned, death, getting taunted, and initiating an attack with associated
/// Animator adjustments.
/// </summary>
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
    [SerializeField]
    UnityEvent startCombatEvent;
    [SerializeField]
    GameObject _model;

    private GameManager gameManager;

    //wwise
    public AK.Wwise.Event angerSFX;
    public AK.Wwise.Event attackSFX;
    public AK.Wwise.Event deathSFX;

    public bool isAngy;
    public Attack currentAttack;

    protected int maxHealth { get; set; }
    protected Attack basicAttack { get; set; }
    protected Attack angyAttack { get; set; }
    protected float moveSpeed { get; set; }
    protected int maxAnger { get; set; }

    protected int health { get; set; }
    protected int anger { get; set; }
    [SerializeField] public EnemyState state;

    protected GameObject player;
    protected HypeManager hypeManager;
    protected FieldOfView fow;

    protected AudioSource audioSource;
    protected bool inHitPause;

    protected NavMeshAgent agent;   // this is the enemy
    protected float range = 21.76f;  // radius of sphere for generating a random point
    protected Transform centrePoint;    // centre of the map (try setting it to the agent for fun?)

    protected GameObject deathBubble;
    protected Coroutine attackCoroutine;

    protected Animator animator;

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
    /// <summary>
    /// Logs this Enemy's information: name, current and max health, current and max anger, and the current
    /// EnemyState enum value.
    /// </summary>
    /// <param name="name"> The name of the enemy type to be logged. </param>
    public void GetEnemyStatus(string name = "Enemy")
    {
        Debug.Log(System.String.Format("{0}(health: {1}/{2}, anger: {3}/{4}, state: {5})", name, health.ToString(), maxHealth.ToString(), anger.ToString(), maxAnger.ToString(), state));
    }

    // invoked when colliding with attack hitbox
    /// <summary>
    /// Takes damage if this Enemy is not already dead. If this Enemy's health depletes to zero or below, sets
    /// the enemy's state to dead with associated animations and changes to the HypeManager hype value.
    /// Otherwise, increases the hype value and stuns this Enemy for a duration of time.
    /// </summary>
    /// <param name="attacker"> The Enemy that unleashed an attack on this Enemy. </param>
    /// <param name="damage"> The amount to deplete this Enemy's health. </param>
    /// <param name="stunTime"> The duration of time to stun this Enemy in seconds. </param>
    public virtual void TakeHit(Enemy attacker, int damage, float stunTime)
    {
        if (state == EnemyState.Dead)
        {
            return;
        }
        if (attackCoroutine != null) {
            StopCoroutine(attackCoroutine);
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

    /// <summary>
    /// Delays this Enemy's actions by a durations of time while tracking the paused state with a flag.
    /// </summary>
    /// <param name="pauseTime"> The duration of time to delay this Enemy in seconds. </param>
    public virtual IEnumerator GetHitPaused(float pauseTime)
    {
        inHitPause = true;
        yield return new WaitForSeconds(pauseTime);
        inHitPause = false;
    }

    // puts enemy in Stunned state for stunTime seconds
    /// <summary>
    /// Triggers the damaged animation associated with this Enemy and sets the state to be stunned for a duration
    /// of time.
    /// </summary>
    /// <param name="stunTime"> The duration of time to stun this Enemy in seconds. </param>
    public virtual IEnumerator GetStunned(float stunTime)
    {
        animator.SetTrigger("takeDamage");

        state = EnemyState.Stunned;
        yield return new WaitForSeconds(stunTime);  // waits for stunTime seconds before continuing

        state = EnemyState.Patrolling;
    }

    // invoked when health falls to/below 0
    /// <summary>
    /// Triggers the death animation and respective SFX associated with this Enemy, disables bookkeeping data
    /// for Enemy behaviours, and increases the HypeManager hype by a specified value before destroying
    /// this GameObject.
    /// </summary>
    public virtual IEnumerator Die()
    {
        if (attackCoroutine != null) {
            deathSFX.Post(gameObject);
            StopCoroutine(attackCoroutine);
        }
        animator.SetTrigger("die");
        animator.SetBool("dead", true);
        deathSFX.Post(gameObject);
        _AngyInd.SetActive(false);
        _AlertInd.SetActive(false);

        hypeManager.IncreaseHype(hypeManager.DEATH_HYPE);
        //deathBubble.SetActive(true);

        fow.active = false;
        basicAttack.Deactivate();  // deactivate attack collider

        state = EnemyState.Dead;
        deathSFX.Post(gameObject);
        yield return new WaitForSeconds(2.5f);  // waits before destroying object

        Destroy(gameObject);
    }

    // invoked when player taunts and enemy is in taunt radius
    /// <summary>
    /// Increments this Enemy's anger, changing this Enemy's state to be angy, triggering associated SFX and
    /// visuals, and swapping the Attack type once the <i> maxAnger </i> threshold is surpassed. Upon
    /// changing the type of Attack, logs the new attack's damage value.
    /// </summary>
    /// <param name="tauntValue"> The value to increase this Enemy's anger. </param>
    public virtual void GetTaunted(int tauntValue = 1)
    {
        anger = anger + tauntValue;
        state = EnemyState.Tracking;
        if (anger >= maxAnger) {
            isAngy = true;
            angerSFX.Post(gameObject);
            _AngyInd.SetActive(true); 
            currentAttack = angyAttack;
            Debug.Log("Damage " + currentAttack.damage);
        }
        else {
            _AngyInd.SetActive(false);
        }
    }

    /// <summary>
    /// Performs the following sequence of operations:
    /// <p> 1. Triggers animations and sets states associated with the Attack's startup and delays by the Attack
    /// <i> startupTime </i>. </p>
    /// <p> 2. Triggers animations and SFX associated with the Attack activation and activates the Attack before
    /// delaying by the Attack <i> activeTime </i>. </p>
    /// <p> 3. Sets the animations and state to recover from the attack, logs the finished Attack status, and
    /// deactivates the Attack before delaying for <i> recoveryTime </i> duration. </p>
    /// <p> 4. Once the recovery time finishes, resets animation data and triggers the walking animation, resets
    /// this Enemy's alert status, and restarts the patrolling state. </p>
    /// <p> If this Enemy's state is changed throughout the sequence, immediately breaks from the attacking
    /// sequence. </p>
    /// </summary>
    /// <param name="attackObj"> The Attack to be activated.</param>
    public IEnumerator Attack(Attack attackObj) {
        // trigger attack animation here
        animator.SetTrigger("startStartup");
        state = EnemyState.Startup;
        Debug.Log("Attacking Time");
        yield return new WaitForSeconds(attackObj.startupTime);
        
        // there's probably a better way to handle the below (& its repetitions)
        if (state != EnemyState.Startup) {
            yield break;
        }

        animator.SetTrigger("startAttack");
        state = EnemyState.Active;
        attackSFX.Post(gameObject);
        // Debug.Log("Active Attack!");
        attackObj.Activate();  // activate attack collider
        yield return new WaitForSeconds(attackObj.activeTime);

        if (state != EnemyState.Active) {
            yield break;
        }

        animator.SetTrigger("startRecovery");
        state = EnemyState.Recovery;
        Debug.Log("Attack All done");
        attackObj.Deactivate();  // deactivate attack collider
        yield return new WaitForSeconds(attackObj.recoveryTime);

        if (state != EnemyState.Recovery) {
            yield break;
        }

        animator.SetTrigger("startWalk");
        animator.ResetTrigger("startStartup");
        _AlertInd.SetActive(false);
        state = EnemyState.Patrolling;
        gameObject.GetComponent<Patrol>().enabled = true;
    }

    /// <summary>
    /// Sets the tracking state and disables the Patrol patrolling logic associated with this Enemy.
    /// </summary>
    protected virtual void PlayerFound()
    {
        state = EnemyState.Tracking;
        gameObject.GetComponent<Patrol>().enabled = false;
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

    /// <summary>
    /// Triggers the walking animation and sets this Enemy's state to patrolling.
    /// </summary>
    protected void OnStartCombat() {
        animator.SetTrigger("startWalk");
        state = EnemyState.Patrolling;
    }

    /// <summary>
    /// Disables Enemy state visuals and deactivates the Attack for this Enemy, setting the state to be passive.
    /// </summary>
    protected void OnBecomePassive() {
        if (attackCoroutine != null) {
            StopCoroutine(attackCoroutine);
        }
        _AngyInd.SetActive(false);
        _AlertInd.SetActive(false);
        animator.SetTrigger("becomeIdle");
        currentAttack.Deactivate();
        state = EnemyState.Passive;
    }

    /// <summary>
    /// Initializes all the class bookkeeping data associated with an Enemy such as:
    /// <p> The current and max <b> health </b>. </p>
    /// <p> The current and max <b> anger </b>. </p>
    /// <p> The <b> movement speed </b>. </p>
    /// <p> The basic, angy, and current <b> types of Attacks </b>. </p>
    /// <p> The core game system managers. </p>
    /// <p> The components attached to the associated GameObject. </p>
    /// <p> The <b> player </b> GameObject. </p>
    /// </summary>
    protected virtual void Start()
    {
        maxHealth = _maxHealth;
        health = _maxHealth;
        maxAnger = _maxAnger;
        anger = 0;
        moveSpeed = _moveSpeed;
        basicAttack = _basicAttack;
        angyAttack = _angyAttack;
        currentAttack = _basicAttack;
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        animator = _model.GetComponent<Animator>();
        // deathBubble = _deathBubble;
        // deathBubble.SetActive(false);
        player = GameObject.FindWithTag("Player");
        hypeManager = GameObject.Find("Game Manager").GetComponent<HypeManager>();
        fow = gameObject.GetComponent<FieldOfView>();
        agent = GetComponent<NavMeshAgent>();
        centrePoint = agent.transform;
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(DelayStart());
        gameManager.startCombatEvent.AddListener(OnStartCombat);
        gameManager.startTutorialEvent.AddListener(OnStartCombat);
        gameManager.stopCombatEvent.AddListener(OnBecomePassive);
    }

    /// <summary>
    /// Delays before checking that the GameManager level state is currently active to activate the core Enemy
    /// activity. If the level state isn't ready for combat, sets this Enemy to be in the passive state.
    /// </summary>
    IEnumerator DelayStart() {
        state = EnemyState.Spawning;
        yield return new WaitForSecondsRealtime(0.5f);
        if (gameManager.state == GameState.Combat) {
            animator.SetTrigger("startWalk");
            state = EnemyState.Patrolling;
        }
        else {
            state = EnemyState.Passive;
        }
    }
}