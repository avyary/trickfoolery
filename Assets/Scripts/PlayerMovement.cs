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
    
    //Player inputs
    private float _playerInputVertical;
    private float _playerInputHorizontal;
    private Vector3 _movementDirection;
    
    
    private float speedChangeFactor = 50f;
    public float dashCdTimer = 0;
    public float tauntCdTimer = 0;
    private float lastDesiredMoveSpeed;
    private float desiredMoveSpeed;
    private float walkspeed;
    private Vector3 gravityVelocity;
    private float _gravity = -9.81f;

    private Rigidbody _rb;
    private CharacterController _movementController;

    private HypeManager hypeManager;
    private GameManager gameManager;

    //Speed of different movement abilities
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

    public AbilityState state;
    public FieldOfView fov;

    public enum AbilityState
    {
        walking,
        dashing,
        taunting
    }

    private bool dashing;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _movementController = GetComponent<CharacterController>();
        hypeManager = GameObject.FindWithTag("GameManager").GetComponent<HypeManager>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        fov = gameObject.GetComponent<FieldOfView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.isPaused) {
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
            {
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

        foreach (var enemy in inRange)
        {
            Debug.Log("test");
            Debug.Log(enemy);
            enemy.gameObject.GetComponent<Enemy>().GetTaunted();
        }
        
        while (Time.time < startTime + TAUNTTIME)
        {
            state = AbilityState.taunting;
            yield return null;
        }
        
        
        Debug.Log("Not Taunting");
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
    


    /// <summary>
    /// Method <c>Dash</c> applies dash when spacebar is pressed.
    /// </summary>
    // IEnumerator Dash()
    //
    // {
    //     float time;
    //     float boostFactor = speedChangeFactor;
    //     float startTime = Time.time;
    //     
    //     
    //     time = 0;
    //     float difference = Mathf.Abs(dashSpeed - walkSpeed);
    //     while (time < difference)
    //     {
    //         moveSpeed = Mathf.Lerp(dashSpeed, walkSpeed, time / difference);
    //         time += Time.deltaTime * boostFactor;
    //         yield return null;
    //     }
    //
    //     moveSpeed = walkSpeed;
    //     speedChangeFactor = 1f;
    // }
}
