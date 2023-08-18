using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// *******************************************************************************************
// PlayerMovement
//*******************************************************************************************
/// <summary>
/// Handles player mechanics in response to player input and contains logic associated
/// with the player states linked to the UI and Game state systems.
/// </summary>
public class PlayerMovement : MonoBehaviour

{
    /// <summary>
    /// Audio implementation stuff starts with AK.Wwise 
    /// </summary>

    public AK.Wwise.Event dashSFX;
    public AK.Wwise.Event playerDeathSFX;
    public AK.Wwise.Event playerHurtSFX;

    [SerializeField] private ParticleSystem DashParticle;
    [SerializeField]
    public float dodgeRadius;
    [SerializeField]
    private LayerMask attackMask;
    
    //Player inputs
    private float _playerInputVertical;
    private float _playerInputHorizontal;
    private Vector3 _movementDirection;
    
    private float speedChangeFactor = 50f;
    public float dashCdTimer = 0; //for debugging
    public float tauntCdTimer = 0; //for debugging
    private float lastDesiredMoveSpeed;
    private float desiredMoveSpeed;
    private float walkspeed;
    private Vector3 gravityVelocity;
    private float _gravity = -9.81f;
    public float health; //for debugging
    public bool isInvincible = false;
    
    private CharacterController _movementController;

    private HypeManager hypeManager;
    public GameManager gameManager;
    private UIManager uiManager;
    private Material tauntMat;
    private Material originalMat;

    // movement direction calculation
    private GameObject camera;
    private Vector3 camForward;
    private Vector3 camRight;

    //Speed of different player abilities
    [SerializeField]
    private float WALKSPEED = 10f;
    [SerializeField]
    private float DASHSPEED = 50f;
    [SerializeField]
    private float DASHTIME = 0.15f;
    [SerializeField]
    private float DASHCD = 0.5f;
    [SerializeField]
    private float TAUNTTIME = 1f;
    [SerializeField]
    private float TAUNTCD = 1f;
    [SerializeField]
    private float GRAVITY_MULTIPLIER = 1f;
    [SerializeField]
    private float POSTDASH;
    public float MAX_HEALTH = 100;

    [SerializeField]
    private float damageFlashTime;
    [SerializeField]
    private float hitInvincibility;

    [SerializeField]
    private GameObject tomRenderObj;
    private SkinnedMeshRenderer tomRender;
    [SerializeField]
    private Material damageMat;
    [SerializeField]
    private Material invincibilityMat;

    public AbilityState state;
    public FieldOfView fov;

    public Animator tomAnimator;

    private Coroutine tauntCoroutine;

    public enum AbilityState
    {
        walking,
        dashing,
        taunting,
        damage,
        dead
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _movementController = GetComponent<CharacterController>();
        hypeManager = GameObject.Find("Game Manager").GetComponent<HypeManager>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        uiManager = GameObject.Find("Game Manager").GetComponent<UIManager>();
        fov = gameObject.GetComponent<FieldOfView>();
        health = MAX_HEALTH;
        tauntMat = Resources.Load("TauntColor", typeof(Material)) as Material;
        tomRender = tomRenderObj.GetComponent<SkinnedMeshRenderer>();
        originalMat = tomRender.material;
        
        tomAnimator = GameObject.Find("Idle_TOM").GetComponent<Animator>();

        camera = GameObject.FindWithTag("MainCamera");
        camForward = camera.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();
        camRight = camera.transform.right;
        camRight.y = 0f;
        camRight.Normalize();

        transform.rotation = Quaternion.LookRotation(camForward * -1);
        gameManager.stopCombatEvent.AddListener(OnBecomePassive);
    }

    /// <summary>
    /// Invokes the player dash mechanic and associated particle systems if the GameManager playerInput flag is
    /// enabled.
    /// </summary>
    /// <param name="value"> The Action value received from player input. </param>
    public void OnDodge(InputValue value) {
        if (gameManager.playerInput) {
            StartCoroutine(Dash());
            DashParticle.Play();
            StartCoroutine(WaitForSecondsAndStopParticles(0.1f, DashParticle));
        }

    }

    /// <summary>
    /// Invokes the player taunt mechanic if the GameManager playerInput flag is enabled and the taunt cooldown
    /// has completed.
    /// </summary>
    /// <param name="value"> The Action value received from player input. </param>
    public void OnTaunt(InputValue value) {
        if (gameManager.playerInput && tauntCdTimer <= 0) {
                tauntCoroutine = StartCoroutine(InitiateTaunt());
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.playerInput) {
            if (dashCdTimer > 0) {
                dashCdTimer -= Time.deltaTime;
            }
            if (tauntCdTimer > 0) {
                tauntCdTimer -= Time.deltaTime;
            }
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            hypeManager.IncreaseHype(100);
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            TakeHit(100);
        }

        ApplyGravity();
    }

    private void FixedUpdate()
    {
        if (gameManager.playerInput) {
            if (state == AbilityState.walking) 
                _movementController.Move(_movementDirection * WALKSPEED * Time.deltaTime);
            if (state == AbilityState.dashing)
                _movementController.Move(transform.forward * DASHSPEED * Time.deltaTime);
        }
    }

    /// <summary>
    /// Sets the player animation to become idle.
    /// </summary>
    private void OnBecomePassive() {
        tomAnimator.SetBool("isRunning", false);
    }

    /// <summary>
    /// If the dash cooldown has completed, cancels any taunts that may be currently processed, resets the dash
    /// cooldown, and makes the player dash, triggering associated animations and SFX, potentially increasing
    /// the HypeManager hype value, activating temporary invincibility, and setting the dashing player state.
    /// Upon completion, resets the player state to walking.
    /// </summary>
    IEnumerator Dash()
    {   
        if (dashCdTimer <= 0f) 
        {
            if (tauntCoroutine != null) 
            {
                print("cancelled taunt");
                StopCoroutine(tauntCoroutine);
            }
            StartCoroutine(CheckHypeDash());
            StartCoroutine(InvincibilityFrames(DASHTIME));
            dashCdTimer = DASHCD;
            float startTime = Time.time;
            tomAnimator.SetTrigger("StartDodge");
            dashSFX.Post(gameObject);

            while (Time.time < startTime + DASHTIME)
            {
                state = AbilityState.dashing;
                //TODO: Add momentum to make dashing a little more fluid. 
                yield return null;
            }

            state = AbilityState.walking;
        }
        
    }

    /// <summary>
    /// Increases the HypeManager hype value once during the dash frames if the dash near misses the Enemy's
    /// Attack.
    /// </summary>
    IEnumerator CheckHypeDash() {
        float startTime = Time.time;
        Vector3 origin = transform.position;
        bool gotHype = false;
        while (Time.time < startTime + (DASHTIME + POSTDASH))
        {
            if (!gotHype && IsCloseDash(origin))
            {
                gotHype = true;
                hypeManager.IncreaseHype(hypeManager.DODGE_HYPE);
            }
            yield return null;
        }
    }

    /// <summary>
    /// Sets the taunting player state and associated animations and delays for some seconds. After the delay,
    /// invokes the taunt to increase the HypeManager hype value and anger nearby Enemy types before delaying
    /// once again. Upon completion, resets the taunt cooldown and sets the player state to walking.
    /// </summary>
    IEnumerator InitiateTaunt() {
        state = AbilityState.taunting;
        tomAnimator.SetTrigger("StartTaunt");
        yield return new WaitForSeconds(1f);
        Taunt();
        yield return new WaitForSeconds(0.56f);
        state = AbilityState.walking;
        tauntCdTimer = TAUNTCD;
    }
    
    /// <summary>
    /// Tracks all Enemy types in range of the player's FieldOfView. For each Enemy type in range, increases
    /// the HypeManager hype value and taunts that Enemy.
    /// </summary>
    public void Taunt()
    {
        List<Collider> inRange = fov.FindVisibleTargets();
        foreach (var enemy in inRange)
        {
            hypeManager.IncreaseHype(5f);
            enemy.gameObject.GetComponent<Enemy>().GetTaunted();
        }
    }
    
    /// <summary>
    /// Returns the status of if any Attacks fall within the ranges of spheres drawn from the player
    /// position and a specified origin, both with extents defined by the <i> dodgeRadius </i>.
    /// </summary>
    /// <param name="origin"> The centre of a spherical volume to detect Attacks.</param>
    bool IsCloseDash(Vector3 origin)
    {
        Collider[] attacksInRange = Physics.OverlapSphere(transform.position, dodgeRadius, attackMask);
        Collider[] attacksAtOrigin = Physics.OverlapSphere(origin, dodgeRadius, attackMask);
        return (attacksInRange.Length + attacksAtOrigin.Length > 0);
    }
    
    /// <summary>
    /// Applies movement to the player GameObject along the y-axis defined by <i> _gravity </i> multiplied
    /// by the <i> GRAVITY_MULTIPLIER </i>.
    /// </summary>
    private void ApplyGravity()
    {
        gravityVelocity = Vector3.zero;
        gravityVelocity.y += _gravity * GRAVITY_MULTIPLIER;
        _movementController.Move(gravityVelocity * Time.deltaTime);
    }

    /// <summary>
    /// Takes damage if the player is not dead nor currently invincible. If the player's health depletes to zero
    /// or below, updates the UIManager health UI and triggers the losing condition of the game. Otherwise,
    /// triggers damaged animations and SFX, stuns the player, and activates temporary invincibility.
    /// </summary>
    /// <param name="damage"> The amount to deplete the player's health. </param>
    /// <param name="attacker"> The Enemy that unleashed an attack on the player. </param>
    public void TakeHit(int damage, Enemy attacker = null)
    {
        if (gameManager.isGameOver || isInvincible)
        {
            return;
        }
        
        health -= damage; //TODO: change once attack damages have been tweaked
        print(health);
        if (health <= 0)
        {
            uiManager.UpdateHealth(0f);
            StartCoroutine(Die());
        }
        else
        {
            tomAnimator.SetTrigger("GetHurt");
            uiManager.UpdateHealth((float) health / MAX_HEALTH);
            playerHurtSFX.Post(gameObject);
            StartCoroutine(GetStunned());
            StartCoroutine(InvincibilityFrames(hitInvincibility));
        }
    }

    // placeholder
    /// <summary>
    /// Flashes the player GameObject different colors by changing the model material between delays defined
    /// by <i> damageFlashTime </i> and <i> hitInvincibility </i> before resetting the original material. 
    /// </summary>
    IEnumerator FlashOnHit() {
        tomRender.material = damageMat;
        yield return new WaitForSeconds(damageFlashTime);
        tomRender.material = invincibilityMat;
        yield return new WaitForSeconds(hitInvincibility - damageFlashTime);
        tomRender.material = originalMat;
    }

    /// <summary>
    /// Sets the player's state to damaged and freezes the player for a duration of time before resetting
    /// the walking state.
    /// </summary>
    IEnumerator GetStunned() {
        state = AbilityState.damage;
        yield return new WaitForSeconds(0.75f);
        state = AbilityState.walking;
    }

    /// <summary>
    /// Updates the UIManager health UI to be depleted and sets the player state to dead, triggering associated
    /// animations and SFX before triggering the loss of the game.
    /// </summary>
    IEnumerator Die()
    {
        uiManager.UpdateHealth(0f);
        state = AbilityState.dead;
        tomAnimator.SetTrigger("Die");
        playerDeathSFX.Post(gameObject);
        StartCoroutine(gameManager.GameOverLose());
        
        yield return null;
    }

    /// <summary>
    /// Adjusts the player GameObject visuals by changing the material throughout the invincibility
    /// duration, resetting the material and invincible flag upon completion.
    /// </summary>
    /// <param name="time"> The duration of time to stay invincible in seconds. </param>
    IEnumerator InvincibilityFrames(float time)
    {
        isInvincible = true;
        tomRender.material = invincibilityMat;
        yield return new WaitForSeconds(time);
        tomRender.material = originalMat;
        isInvincible = false;
    }

    /// <summary>
    /// Invokes the player movement mechanic if the GameManager playerInput flag is enabled, triggering
    /// associated movement animations with input. If no movement is detected, adjust the animator to
    /// become idle.
    /// </summary>
    /// <param name="value"> The Action value received from player input. </param>
    public void OnMove(InputValue value) {
        if (gameManager.playerInput) {
            Vector2 input = value.Get<Vector2>();
            _movementDirection = camForward * input.y + camRight * input.x;
            if (input == Vector2.zero) {
                tomAnimator.SetBool("isRunning", false);
                return;
            }
            if (state == AbilityState.walking) {
                tomAnimator.SetBool("isRunning", true);
                // camForward * _playerInputVertical + camRight * _playerInputHorizontal;
                transform.forward = _movementDirection;
            }
        }
    }

    /// <summary>
    /// Sets the player GameObject material and delays for a duration of time. If the provided time is nonzero,
    /// resets the material after the delay.
    /// </summary>
    /// <param name="newMat"> The new material to set on the player GameObject. </param>
    /// <param name="time"> The duration of time to delay. </param>
    IEnumerator ChangeMaterial(Material newMat, float time = 0)
    {
        if (time == 0) {
            tomRender.material = newMat;
            // GetComponent<MeshRenderer>().material = newMat;
            yield return new WaitForSeconds(0);
        }
        else {
            tomRender.material = newMat;
            // GetComponent<MeshRenderer>().material = newMat;
            yield return new WaitForSeconds(time);
            tomRender.material = originalMat;
        }
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
}   


