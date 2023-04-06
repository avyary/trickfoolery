using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Logic for basic game movement and dash mechanic.
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

    private Animator tomAnimator;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            StartCoroutine(Die());
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            TakeHit(10);
        }
        if (state == AbilityState.dead) {
            return;
        }
        if (gameManager.state == GameState.Combat || gameManager.state == GameState.Tutorial) {
            //Calculate Inputs for player movement
            _playerInputVertical = Input.GetAxisRaw("Vertical");
            _playerInputHorizontal = Input.GetAxisRaw("Horizontal");
            _movementDirection = camForward * _playerInputVertical + camRight * _playerInputHorizontal;
            _movementDirection.Normalize();

            if (_movementDirection != Vector3.zero)
            {
                tomAnimator.SetBool("isRunning", true);
                if (state == AbilityState.walking) {
                    transform.forward = _movementDirection;
                }    else {
                tomAnimator.SetBool("isRunning", false);
            }
            }
        

            if (Input.GetButtonDown("Dash") && dashCdTimer <= 0)
            {
                StartCoroutine(Dash());
                DashParticle.Play();
                StartCoroutine(WaitForSecondsAndStopParticles(0.1f, DashParticle));
            }

            if (Input.GetButton("Taunt") && tauntCdTimer <= 0)
            {
                StartCoroutine(Taunt());
            }

            //Process the cooldown timer for dashing
            if (dashCdTimer > 0)
                dashCdTimer -= Time.deltaTime;
            if (tauntCdTimer > 0)
                tauntCdTimer -= Time.deltaTime;
        }

        ApplyGravity();
    }

    private void FixedUpdate()
    {
        if (gameManager.state == GameState.Combat || gameManager.state == GameState.Tutorial) {
            if (state == AbilityState.walking) 
                _movementController.Move(_movementDirection * WALKSPEED * Time.deltaTime);
            if (state == AbilityState.dashing)
                _movementController.Move(transform.forward * DASHSPEED * Time.deltaTime);
        }
    }

    IEnumerator Dash()
    {
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


    IEnumerator CheckHypeDash() {
        float startTime = Time.time;
        bool gotHype = false;
        while (Time.time < startTime + (DASHTIME + POSTDASH))
        {
            if (!gotHype && IsCloseDash())
            {
                gotHype = true;
                hypeManager.IncreaseHype(hypeManager.DODGE_HYPE);
            }
            yield return null;
        }
    }
    
    
    IEnumerator Taunt()
    {
        state = AbilityState.taunting;
        
        float startTime = Time.time;

        List<Collider> inRange = fov.FindVisibleTargets();
        foreach (var enemy in inRange)
        {
            enemy.gameObject.GetComponent<Enemy>().GetTaunted();
        }
        
        while (Time.time < startTime + TAUNTTIME)
        {
            state = AbilityState.taunting;
            yield return null;
        }
        
        state = AbilityState.walking;
        tauntCdTimer = TAUNTCD;
    }
    
    
    bool IsCloseDash()
    {
        Collider[] attacksInRange = Physics.OverlapSphere(transform.position, dodgeRadius, attackMask);
        return (attacksInRange.Length > 0);
    }
    
    private void ApplyGravity()
    {
        gravityVelocity = Vector3.zero;
        gravityVelocity.y += _gravity * GRAVITY_MULTIPLIER;
        _movementController.Move(gravityVelocity * Time.deltaTime);
    }


    public void TakeHit(int damage, Enemy attacker = null)
    {
        if (state == AbilityState.dead || isInvincible || gameManager.state == GameState.Tutorial)
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
    IEnumerator FlashOnHit() {
        tomRender.material = damageMat;
        yield return new WaitForSeconds(damageFlashTime);
        tomRender.material = invincibilityMat;
        yield return new WaitForSeconds(hitInvincibility - damageFlashTime);
        tomRender.material = originalMat;
    }

    IEnumerator GetStunned() {
        state = AbilityState.damage;
        yield return new WaitForSeconds(0.75f);
        state = AbilityState.walking;
    }

    IEnumerator Die()
    {
        state = AbilityState.dead;
        GetComponent<MeshRenderer>().material.color = Color.black;
        tomAnimator.SetTrigger("Die");
        playerDeathSFX.Post(gameObject);
        yield return new WaitForSeconds(3f);
        gameManager.GameOverLose();
        yield return null;
    }

    IEnumerator InvincibilityFrames(float time)
    {
        isInvincible = true;
        tomRender.material = invincibilityMat;
        yield return new WaitForSeconds(time);
        tomRender.material = originalMat;
        isInvincible = false;
    }

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
    }private IEnumerator WaitForSecondsAndStopParticles(float seconds, ParticleSystem particles) {
        yield return new WaitForSeconds(seconds);
        particles.Stop();
    } 
     }   


