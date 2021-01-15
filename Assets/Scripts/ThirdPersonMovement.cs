using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
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
    [SerializeField] [Range(-10, 0)] float minFallVelocity; //Once this velocity is hit, isFall becomes true

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


    /*
    Vector3 rightFootPosition, leftFootPosition, leftFootIKPosition, rightFootIKPosition;
    Quaternion leftFootIKRotation, rightFootIKRotation;
    float lastPelvisPositionY, lastRightFootPositionY, lastLeftFootPositionY;
    [Header("Feet Grounder")]
    [SerializeField] bool enableFeetIK = true;
    [SerializeField][Range(0,2)] float heightFromGroundRayCast = 1.14f;
    [SerializeField] [Range(0, 2)] float rayCastDownDistance = 1.5f;
    [SerializeField] LayerMask environmentLayer;
    [SerializeField] float pelvisOffSet = 0f;
    [SerializeField] [Range(0, 1)] float pelvisUpAndDownSpeed= 0.28f;
    [SerializeField] float feetToIKPositionSpeed = 0.5f;
    public string leftFootAnimVariableName = "LeftFootCurve";
    public string rightFootAnimVariableName = "RightFootCurve";

    public bool useProIKFeature = false;
    public bool showSolverDebug = true;
    */

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        isJumping = false;
        Cursor.lockState = CursorLockMode.Locked;
        moveSpeed = walkSpeed;
        lastPos = transform.position; // initialize lastPos
    }

    // Update is called once per frame
    void Update()
    {
        FallingManager();
        PlayerMovement();
    }

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

    #region Movement
    void PlayerMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y <= 0)
        {
            
            if (isJumping)
            {
                GetComponent<AnimationStateController>().Land();
                isJumping = false;
                StartCoroutine(CoolDownFunction());
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
            GetComponent<AnimationStateController>().Jump();
            StartCoroutine(Jump());        
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
    #endregion

    /*
    #region FeetGrounding

    /// <summary>
    /// We are updating the AdjustFeetTarget Method and also finding the position of each foot inside our Solver Position.
    /// </summary>
    private void FixedUpdate()
    {
        if (!enableFeetIK)
        {
            return;
        }
        if (!anim) { return; }
        AdjustFeetTarget(rightFootPosition, HumanBodyBones.RightFoot);
        AdjustFeetTarget(leftFootPosition, HumanBodyBones.LeftFoot);

        FeetPositionSolver(rightFootPosition, ref rightFootIKPosition,ref rightFootIKRotation);
        FeetPositionSolver(leftFootIKPosition, ref leftFootIKPosition,ref  leftFootIKRotation);

        
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (!enableFeetIK) { return; }
        if (!anim) { return; }
        MovePelvisHeight();
        
        //right foot ik position and rotation -- utilise the pro features in here
        anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
        if (useProIKFeature)
        {
            anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, anim.GetFloat(rightFootAnimVariableName));
        }
        MoveFeetToIKPoint(AvatarIKGoal.RightFoot, rightFootIKPosition, rightFootIKRotation, ref lastRightFootPositionY);

        //left foot ik position and rotation -- utilise the pro features in here
        anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
        if (useProIKFeature)
        {
            anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, anim.GetFloat(leftFootAnimVariableName));
        }
        MoveFeetToIKPoint(AvatarIKGoal.LeftFoot, leftFootIKPosition, leftFootIKRotation, ref lastLeftFootPositionY);

    }
    #endregion

    
    #region FeetGroundingMethods

    /// <summary>
    /// Move feet to ik position
    /// </summary>
    /// <param name="foot"></param>
    /// <param name="positionIKHolder"></param>
    /// <param name="rotationIKHolder"></param>
    /// <param name="lastFootPositionY"></param>
    void MoveFeetToIKPoint(AvatarIKGoal foot, Vector3 positionIKHolder, Quaternion rotationIKHolder, ref float lastFootPositionY)
    {
        Vector3 targetIKPosition = anim.GetIKPosition(foot);
        
        if(positionIKHolder != Vector3.zero)
        {
            targetIKPosition = transform.InverseTransformPoint(targetIKPosition);
            positionIKHolder = transform.InverseTransformPoint(positionIKHolder);

            float yVariable = Mathf.Lerp(lastLeftFootPositionY, positionIKHolder.y, feetToIKPositionSpeed);
            targetIKPosition.y += yVariable;

            lastFootPositionY = yVariable;

            targetIKPosition = transform.TransformPoint(targetIKPosition);

            anim.SetIKRotation(foot, rotationIKHolder);
        }

        anim.SetIKPosition(foot, targetIKPosition);

    }
    /// <summary>
    /// Move height of pelvis
    /// </summary>
    private void MovePelvisHeight()
    {
        if(rightFootIKPosition == Vector3.zero || leftFootIKPosition ==Vector3.zero || lastPelvisPositionY == 0)
        {
            lastPelvisPositionY = anim.bodyPosition.y;
            return;
        }

        float leftOffsetPosition = leftFootIKPosition.y - transform.position.y;
        float rightOffsetPosition = rightFootIKPosition.y - transform.position.y;

        float totalOffset = (leftOffsetPosition < rightOffsetPosition) ? leftOffsetPosition : rightOffsetPosition;

        Vector3 newPelvisPosition = anim.bodyPosition + Vector3.up * totalOffset;
        newPelvisPosition.y = Mathf.Lerp(lastPelvisPositionY, newPelvisPosition.y, pelvisUpAndDownSpeed);

        anim.bodyPosition = newPelvisPosition;
        lastPelvisPositionY = anim.bodyPosition.y;
    }

    /// <summary>
    /// Locating feet position via raycast and then solving
    /// </summary>
    /// <param name="fromSkyPosition"></param>
    /// <param name="feetIKPosition"></param>
    /// <param name="feetIKRotation"></param>
    private void FeetPositionSolver(Vector3 fromSkyPosition, ref Vector3 feetIKPosition,ref Quaternion feetIKRotation)
    {
        //raycast handling section
        RaycastHit feetOutHit;

        if (showSolverDebug)
        {
            Debug.DrawLine(fromSkyPosition, fromSkyPosition + Vector3.down * (rayCastDownDistance + heightFromGroundRayCast), Color.yellow);
        }

        if (Physics.Raycast(fromSkyPosition, Vector3.down, out feetOutHit, rayCastDownDistance + heightFromGroundRayCast, environmentLayer))
        {
            //finding feet ik position from sky position
            feetIKPosition = fromSkyPosition;
            feetIKPosition.y = feetOutHit.point.y + pelvisOffSet;
            feetIKRotation = Quaternion.FromToRotation(Vector3.up, feetOutHit.normal) * transform.rotation;

            return;
        }

        feetIKPosition = Vector3.zero; //if didnt work
    }

    private void AdjustFeetTarget(Vector3 feetPosition, HumanBodyBones foot)
    {
        feetPosition = anim.GetBoneTransform(foot).position;
        feetPosition.y = transform.position.y + heightFromGroundRayCast;
    }
    #endregion
    */

}

