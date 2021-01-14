using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public Animator anim;

    public float walkSpeed = 8f;
    public float sprintSpeed = 12f;
    [SerializeField] float strifeSpeed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    [SerializeField] float jumpDelay = 0.5f;

    public Transform groundCheck;
    public float groundDistance = 0.1f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isJumping;
    bool isGrounded;
    static bool canJump = true;
    public float jumpCooldown = 0.5f;


    Vector3 rightFootPosition, leftFootPosition, leftFootIKPosition, rightFootIKPosition;
    Quaternion leftFootIKRotation, rightFoorIKRotation;
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


    private void Start()
    {
        isJumping = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }


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
            walkSpeed = sprintSpeed;
        }
        
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            walkSpeed = 4f;
        }



        if (direction.magnitude >= 0.1f)
        {

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * walkSpeed * Time.deltaTime);

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

        FeetPositionSolver(rightFootPosition, ref rightFootIKPosition,rightFoorIKRotation);
        FeetPositionSolver(leftFootIKPosition, ref leftFootIKPosition, leftFootIKRotation);

        
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (!enableFeetIK) { return; }
        if (!anim) { return; }
        MovePelvisHeight();
        //right foot ik position and rotation -- utilise the pro features in here
        
    }
    #endregion


    #region FeetGroundingMethods
    void MoveFeetToIKPoint(AvatarIKGoal foot, Vector3 positionIKHolder, Quaternion rotationIKHolder, ref float lastPositionY)
    {

    }
    private void MovePelvisHeight()
    {

    }

    private void FeetPositionSolver(Vector3 fromSkyPosition, ref Vector3 feetIKPosition, Quaternion feetIKRotation)
    {


    }

    private void AdjustFeetTarget(Vector3 feetPosition, HumanBodyBones foot)
    {

    }
    #endregion
}

