using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{ 
    //Serialized References
    [Header("Camera")]
    [SerializeField] Transform cam;

    [Header("Movement")]
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float sprintSpeed = 12f;
    [SerializeField] float strifeSpeed = 6f;
    [SerializeField] float turnSmoothTime = 0.1f;
    [SerializeField] float gravity = -9.81f;

    [Header("Jump")]
    [SerializeField] float jumpCooldown = 0.5f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float jumpDelay = 0.5f;
    [Header("Fall")]
    [SerializeField] [Range(-10, 0)] float minFallVelocity; //Once this velocity is hit, isFall becomes true
    [SerializeField] [Range(-30, 0)] float minFallDamageVelocity = -10f;
    [SerializeField] float minFallDamage = 10f;
    [SerializeField] float fallDamageMultiplier = 0.5f; //Damage increase per velocity increase
    [SerializeField] int groundLayerNumber = 8;

    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.1f;
    [SerializeField] LayerMask groundMask;

    
    //States
    static bool canJump = true;
    bool isJumping;
    bool isGrounded;
    bool isFalling;
    bool isMoving = false;
    bool isRunning = false;
    float turnSmoothVelocity;
    float moveSpeed;
    Vector3 velocity;
    //For Fall velocity calculation
    Vector3 playerVel; 
    Vector3 lastPos;


    //Cached Component Reference
    CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        isJumping = false;
        Cursor.lockState = CursorLockMode.Locked;
        moveSpeed = walkSpeed;
        lastPos = transform.position; // initialize lastPos for velocity calculations
    }

    // Update is called once per frame
    void Update()
    {
        FallingManager();
        PlayerMovement();
    }
    #region Falling
    void FallingManager()
    {
        var moved = transform.position - lastPos; // update lastPos: 
        lastPos = transform.position; // calculate the velocity: 
        playerVel = moved / Time.deltaTime; 
        float yVelocity = playerVel.y;
        if (yVelocity < minFallVelocity)
        {
            isFalling = true;
        }
        else
        {
            isFalling = false;
        }
    }

    //Fall Damage

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.layer == groundLayerNumber) //Check if collision is with ground
        {
            if (isFalling && playerVel.y <= minFallDamageVelocity) //Check if player is falling and has min fallvelocity
            {

                if(hit.point.y < transform.position.y) //Check if impact point is beneath player
                {
                    Debug.Log("Fall Damage");
                    GetComponent<Health>().TakeDamage(minFallDamage + (-playerVel.y * fallDamageMultiplier));
                }
            }
        }
    }


    #endregion

    #region Movement
    void PlayerMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y <= 0)
        {
            
            if (isJumping)
            {
                //Landing Event
                GetComponent<AnimationStateController>().Land();
                isJumping = false;
                StartCoroutine(CoolDownFunction());
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    moveSpeed = sprintSpeed;
                }
            }



            velocity.y = -2f;

        }
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
        {
            moveSpeed = sprintSpeed;
            

        }
        
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = walkSpeed;
            
        }

        

        if (direction.magnitude >= 0.1f)
        {
            //define if walking or running
            if (moveSpeed == sprintSpeed)
            {
                isRunning = true;
                
            }
            else
            {
                isRunning = false;
            }
            isMoving = true;

            //move caharacter
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
            

        }
        else //if not moving define ismoving false
        {
            isRunning = false;
            isMoving = false;
        }

        if (Input.GetButtonDown("Jump") && isGrounded && canJump)
        {
            StartCoroutine(Jump());
            GetComponent<AnimationStateController>().Jump();        
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        
    }
    IEnumerator Jump()
    {
        yield return new WaitForSeconds(jumpDelay);
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        isJumping = true;
       
    
    }
     
    IEnumerator CoolDownFunction()
    {
        
        canJump = false;
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
        
    }
    #endregion

    #region GetFunctions

    public bool GetIsFalling()
    {
        return isFalling;
    }
    public bool GetIsMoving()
    {
        return isMoving;
    }
    public bool GetIsRunning()
    {
        return isRunning;
    }
    public bool GetIsJumping()
    {
        return isJumping;
    }
    #endregion

   

}

