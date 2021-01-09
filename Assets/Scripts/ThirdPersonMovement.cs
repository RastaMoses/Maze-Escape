using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

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
    private void Start()
    {
        isJumping = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            Debug.Log("is Grounded");
            if (isJumping)
            {
                GetComponent<AnimationStateController>().Land();
                isJumping = false;
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
        else
        {
            walkSpeed = walkSpeed;
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

        if (Input.GetButtonDown("Jump") && isGrounded)
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
}

