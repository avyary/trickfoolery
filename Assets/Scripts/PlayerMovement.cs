using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Logic for basic game movement and dash mechanic.
/// </summary>
public class PlayerMovement : MonoBehaviour

{
    [SerializeField]
    public float dodgeRadius;
    [SerializeField]
    private LayerMask attackMask;
    
   [SerializeField]
    private AudioClip DashSound;


       [SerializeField]
    private AudioClip HurtSound;

    private AudioSource audioSource;
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
    public float MAX_HEALTH = 100;

    public AbilityState state;
    public FieldOfView fov;

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
        hypeManager = GameObject.FindWithTag("GameManager").GetComponent<HypeManager>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        fov = gameObject.GetComponent<FieldOfView>();
        health = MAX_HEALTH;
          audioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.isPaused) {//not sure why this isn't working 
            //Calculate Inputs for player movement
            _playerInputVertical = Input.GetAxisRaw("Vertical");
            _playerInputHorizontal = Input.GetAxisRaw("Horizontal");
            _movementDirection = new Vector3(_playerInputHorizontal, 0, _playerInputVertical);
            _movementDirection.Normalize();

            if (_movementDirection != Vector3.zero && state == AbilityState.walking)
            {
                transform.forward = _movementDirection;
            } ;

            if (Input.GetButton("Jump") && dashCdTimer <= 0)
            {  audioSource.PlayOneShot(DashSound);
                StartCoroutine(Dash());
                 
            }

            if (Input.GetKey("e") && tauntCdTimer <= 0)
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
        if (state == AbilityState.walking) 
            _movementController.Move(_movementDirection * WALKSPEED * Time.deltaTime);
        if (state == AbilityState.dashing)
            _movementController.Move(transform.forward * DASHSPEED * Time.deltaTime);
    }

    IEnumerator Dash()
    {
        dashCdTimer = DASHCD;
        if (IsCloseDash())
        {
            hypeManager.ChangeHype(hypeManager.DODGE_HYPE);
        }

        float startTime = Time.time;

        while (Time.time < startTime + DASHTIME)
        {
            state = AbilityState.dashing;
            //TODO: Add momentum to make dashing a little more fluid. 
            yield return null;
        }
        
        state = AbilityState.walking;
    }
    
    
    IEnumerator Taunt()
    {
        state = AbilityState.taunting;
        
        
        float startTime = Time.time;

        List<Collider> inRange = fov.FindVisibleTargets();
        Debug.Log("Enemies in range");
        Debug.Log(inRange.Count);
        foreach (var enemy in inRange)
        {
            Debug.Log(enemy);
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
        print(attacksInRange.Length);
        return (attacksInRange.Length > 0);
    }
    
    private void ApplyGravity()
    {
        gravityVelocity = Vector3.zero;
        gravityVelocity.y += _gravity * GRAVITY_MULTIPLIER;
        _movementController.Move(gravityVelocity * Time.deltaTime);
    }


    public void TakeHit(int damage)
    {
        if (state == AbilityState.dead || isInvincible)
        {
            return;
        }
        
        health -= 25; //TODO: change once attack damages have been tweaked
        Debug.Log("health: " + health);
        if (health <= 0)
        {
            StartCoroutine(Die());
        }
        else
        {  audioSource.PlayOneShot(HurtSound);
            GetComponent<MeshRenderer>().material.color = Color.red;
            StartCoroutine(InvincibilityFrames());
        }
    }

    IEnumerator Die()
    {
        GetComponent<MeshRenderer>().material.color = Color.black;
        gameManager.GameOverLose();
        yield return null;
    }

    IEnumerator InvincibilityFrames()
    {
        float starttime = Time.time;
        
        while (Time.time <  starttime + 2)
        {
            isInvincible = true;
            yield return null;
        }
        GetComponent<MeshRenderer>().material.color = Color.green;
        isInvincible = false;
    }
}
