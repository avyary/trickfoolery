using System.Collections;
using UnityEngine;

public enum TauntState
{
    passive,
    teleporting
    
}

//*******************************************************************************************
// TauntEnemy
//*******************************************************************************************
/// <summary>
/// Enemy subclass that further specifies TauntEnemy type behaviors such as taunting
/// the player (rolling and dancing) and initiating teleport attacks when sighting
/// the player. Also handles particle effect systems associated with the TauntEnemy
/// and the AoE attack highlighter.
/// </summary>
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

    private Color originalColor;
    
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

    /// <summary>
    /// Extends the parent class initialization of bookkeeping structures with further initialization of
    /// attributes particular to the TauntEnemy subclass, debugging functionality to log this Enemy's
    /// data, and the disabling of the patrolling ability.
    /// </summary>
    protected override void Start() {
        base.Start();
        GetEnemyStatus("TauntEnemy");
        teleporting = false;
        attackcd = attack_cooldown;
        gameObject.GetComponent<Patrol>().enabled = false;
        dancetime = false;
        state = EnemyState.Patrolling;
        originalColor = currentAttack._renderer.material.color;
    }

    /// <summary>
    /// Replays this Enemy's rolling state and animations before teleporting for <i> teleport_time </i>
    /// duration of time, playing associated teleport SFX and particle effects.
    /// </summary>
    /// <param name="strength"> The intensity of the distance this Enemy will keep from or towards the player. </param>
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
    
    /// <summary>
    /// Delays for a duration of time before disabling the ParticleSystem.
    /// </summary>
    /// <param name="seconds"> The duration of time to wait before disabling the particle system in seconds. </param>
    /// <param name="particles"> The ParticleSystem to be disabled. </param>
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

    /// <summary>
    /// Readies the teleport attack state via the TauntAnimationController and strength based on the distance to
    /// the player and progressively adjusts the Area-of-Effect (AoE) highlighter throughout the attack animation.
    /// Upon the triggering of <i> doneAttacking </i> from the TauntAnimationController, paints the AoE red and
    /// activates the respective Attack to damage other actors, then resets the attacking state and attack
    /// cooldown timer.
    /// </summary>
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
        //currentAttack._renderer.material.color = new Color(255, 165, 0, 0.2f);
        currentAttack._renderer.enabled = true;
        currentAttack._renderer.material.color = originalColor;
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
    
    /// <summary>
    /// Translates the associated GameObject in the teleport_direction with the distance determined by the
    /// <i> current_teleport_strength </i>.
    /// </summary>
    public void FixedUpdate()
    {
        if (teleporting)
        {
            transform.Translate(teleport_direction.normalized * current_teleport_strength, Space.World);
        } 
    }
    
    /// <summary>
    /// Delays for a duration of time.
    /// </summary>
    /// <param name="seconds"> The duration of time to wait in seconds. </param>
    private IEnumerator WaitForSecondsAndStopTeleportAnim(float seconds) 
    {
        yield return new WaitForSeconds(seconds);
    }   
}
