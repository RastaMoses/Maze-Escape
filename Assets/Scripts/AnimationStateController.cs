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

    // Update is called once per frame
    void Update()
    {
        
        
        bool forwardPressed = Input.GetKey("w");
        bool backwardPressed = Input.GetKey("s");
        bool leftPressed = Input.GetKey("a");
        bool rightPressed = Input.GetKey("d");
        bool runPressed = Input.GetKey("left shift");
        
        //Walking forward
        if (forwardPressed && velocityZ < 0.5f && !runPressed && !backwardPressed)
        {
            velocityZ += Time.deltaTime * accelerationForward;
        }

        //move backward
        if (backwardPressed && velocityZ > -0.5f && !forwardPressed && !runPressed)
        {
            velocityZ -= Time.deltaTime * accelerationBackward;
        }
        //move left
        if (leftPressed && velocityX > -0.5f && !runPressed)
        {
            velocityX -= Time.deltaTime * accelerationX;
        }
        //move right
        if (rightPressed && velocityX < 0.5f && !runPressed)
        {
            velocityX += Time.deltaTime * accelerationX;
        }

        //stop when not moving left
        if(!leftPressed && velocityX < 0)
        {
            velocityX += Time.deltaTime * decceleration;
        }
        //stop when not moving right
        if (!rightPressed && velocityX > 0)
        {
            velocityX -= Time.deltaTime * decceleration;
        }
        //stop when stop forward movement
        if (!forwardPressed && velocityZ > 0)
        {
            velocityZ -= Time.deltaTime * decceleration;
        }
        if (!backwardPressed && velocityZ < 0)
        {
            velocityZ += Time.deltaTime * decceleration;
        }
        //stop moving Z
        if (!forwardPressed && !backwardPressed && velocityZ != 0 && (velocityZ < 0.05f && velocityZ > 0.05f))
        {
            velocityZ = 0;
        }

        //stop moving X
        if (!leftPressed && !rightPressed && velocityX != 0 && (velocityX < 0.05f && velocityX > 0.05f))
        {
            velocityX = 0;
        }


        //Jumping

        if (Input.GetKeyDown("space"))
        {
            animator.SetBool("isJumping", true);
        }

        if (Input.GetKeyUp("space"))
        {
            animator.SetBool("isJumping", false);
        }



        velocityZ = Mathf.Clamp(velocityZ, -0.5f, 2);
        velocityX = Mathf.Clamp(velocityX, -2, 2);
        animator.SetFloat(velocityXHash, velocityX);
        animator.SetFloat(velocityZHash, velocityZ);
    }
}
