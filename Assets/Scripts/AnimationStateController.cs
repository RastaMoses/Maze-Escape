using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    //Params
    [SerializeField] float accelerationX = 1;
    [SerializeField] float accelerationForward = 1;
    [SerializeField] float accelerationBackward = 0.4f;
    [SerializeField] float sprintAcceleration = 0.6f;
    [SerializeField] float decceleration = 1;
    [SerializeField] float maxWalkVelocity = 0.5f;
    [SerializeField] float maxRunVelocity = 2;

    //States

    int velocityXHash;
    int velocityZHash;
    float velocityX;
    float velocityZ;



    //Cached components
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        velocityXHash = Animator.StringToHash("velocity X");
        velocityZHash = Animator.StringToHash("velocity Z");
    }

    void ChangeVelocity(bool forwardPressed, bool backwardPressed, bool leftPressed, bool rightPressed, bool runPressed, float currentMaxVelocity)
    {
        //Walking forward
        if (forwardPressed && velocityZ < currentMaxVelocity && !backwardPressed)
        {
            velocityZ += Time.deltaTime * accelerationForward;
        }

        //move backward
        if (backwardPressed && velocityZ > -0.5f && !forwardPressed)
        {
            velocityZ -= Time.deltaTime * accelerationBackward;
        }
        //move left
        if (leftPressed && velocityX > -0.5f)
        {
            velocityX -= Time.deltaTime * accelerationX;
        }
        //move right
        if (rightPressed && velocityX < 0.5f)
        {
            velocityX += Time.deltaTime * accelerationX;
        }
    }

    void LockOrResetVelocity(bool forwardPressed, bool backwardPressed, bool leftPressed, bool rightPressed, bool runPressed, float currenMaxVelocity)
    {

        //stop when not moving left
        if (!leftPressed && velocityX < 0)
        {
            velocityX += Time.deltaTime * decceleration;
        }
        //stop when not moving right
        if (!rightPressed && velocityX > 0)
        {
            velocityX -= Time.deltaTime * decceleration;
        }
        //stop when stop forward movement
        if (!forwardPressed && velocityZ > 0 || (forwardPressed && backwardPressed && velocityZ > 0))
        {
            velocityZ -= Time.deltaTime * decceleration;
        }
        //stop when backward stop
        if (!backwardPressed && velocityZ < 0 || (forwardPressed && backwardPressed && velocityZ < 0))
        {
            velocityZ += Time.deltaTime * decceleration;
        }
        //lock stop moving Z
        if (!forwardPressed && !backwardPressed && velocityZ != 0 && (velocityZ < 0.05f && velocityZ > 0.05f))
        {
            velocityZ = 0;
        }

        //lock stop moving X
        if (!leftPressed && !rightPressed && velocityX != 0 && (velocityX < 0.05f && velocityX > 0.05f))
        {
            velocityX = 0;
        }


        Mathf.Clamp(velocityZ, -0.5f, 2);
        Mathf.Clamp(velocityX, -0.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
        
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool backwardPressed = Input.GetKey(KeyCode.S);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        //set current maxVelocity
        float currentMaxVelocity = runPressed ? maxRunVelocity : maxWalkVelocity;


        LockOrResetVelocity(forwardPressed, backwardPressed, leftPressed, rightPressed, runPressed,currentMaxVelocity);
        ChangeVelocity(forwardPressed, backwardPressed, leftPressed, rightPressed, runPressed, currentMaxVelocity);
        //set params in animator
        animator.SetFloat(velocityXHash, velocityX);
        animator.SetFloat(velocityZHash, velocityZ);
    }

    public void Jump()
    {
        animator.SetBool("isJumping", true);
    }

    public void Land()
    {
        Debug.Log("Landed animation");
        animator.SetBool("isJumping", false);
    }
}
