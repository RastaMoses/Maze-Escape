using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    //Params
    [SerializeField] float accelerationForward = 1;
    [SerializeField] float sprintAcceleration = 0.6f;
    [SerializeField] float decceleration = 1;
    [SerializeField] float maxWalkVelocity = 0.5f;
    [SerializeField] float maxRunVelocity = 2;
    [SerializeField] float velocityToSkipLandAnimation = 0.1f;
    [SerializeField] float timeIdleUntilNewAnimation = 10;
    [SerializeField] int amountOfIdleAnimations;
    //States
    bool idle;
    float idleTime;
    float velocityX; 
    float velocityZ;
    float currentMaxVelocity;
    //Hashes
    int velocityXHash;
    int velocityZHash;
    int isJumpingHash;
    int playLandingHash;
    int idleAnimationHash;



    //Cached components
    Animator animator;
    SFXControllerPlayer sfx;

    // Start is called before the first frame update
    void Start()
    {
        idle = true;
        idleTime = 0;

        animator = GetComponent<Animator>();
        sfx = GetComponent<SFXControllerPlayer>();

        //Set Hashes for better performance
        velocityXHash = Animator.StringToHash("velocity X");
        velocityZHash = Animator.StringToHash("velocity Z");
        isJumpingHash = Animator.StringToHash("isJumping");
        playLandingHash = Animator.StringToHash("playLanding");
        idleAnimationHash = Animator.StringToHash("idleAnimation");

    }

    void ChangeVelocity(bool forwardPressed, bool backwardPressed, bool leftPressed, bool rightPressed, bool runPressed, float currentMaxVelocity)
    {
        //Walking forward
        if ((forwardPressed || backwardPressed || leftPressed || rightPressed) && velocityZ < currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * accelerationForward;
            
        }
        
        //runnning

        if (runPressed && (forwardPressed || backwardPressed|| leftPressed|| rightPressed) && velocityZ < currentMaxVelocity && !animator.GetBool(isJumpingHash))
        {
            velocityZ += Time.deltaTime * sprintAcceleration;
        }
        

    }

    void LockOrResetVelocity(bool forwardPressed, bool backwardPressed, bool leftPressed, bool rightPressed, bool runPressed, float currenMaxVelocity)
    {
 

        //deccelerate when stop forward movement button
        if (!forwardPressed && !backwardPressed && !leftPressed && !rightPressed && velocityZ > 0 || (forwardPressed && backwardPressed && velocityZ > 0) || (leftPressed && rightPressed && velocityZ > 0))
        {
            velocityZ -= Time.deltaTime * decceleration;
        }



        //lock stop moving Z
        if (!forwardPressed && !backwardPressed && !rightPressed && !leftPressed && velocityZ != 0 && (velocityZ < 0.05f && velocityZ > -0.05f))
        {

            velocityZ = 0;
        }



        //lock stop runnning, when not run pressed, but still forward pressed
        if (!runPressed && (forwardPressed || backwardPressed || leftPressed || rightPressed) && velocityZ > maxWalkVelocity)
        {
            velocityZ -= Time.deltaTime * decceleration;
        }


        Mathf.Clamp(velocityZ, -0.5f, maxRunVelocity);
    }

    // Update is called once per frame
    void Update()
    {
        
        //Assign Inputs
        bool forwardPressed = Input.GetKey(KeyCode.W);
        
        bool backwardPressed = Input.GetKey(KeyCode.S);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        //set current maxVelocity
        if (!animator.GetBool(isJumpingHash))
        {

            currentMaxVelocity = runPressed ? maxRunVelocity : maxWalkVelocity;
        }
        



        ChangeVelocity(forwardPressed, backwardPressed, leftPressed, rightPressed, runPressed, currentMaxVelocity);

        LockOrResetVelocity(forwardPressed, backwardPressed, leftPressed, rightPressed, runPressed, currentMaxVelocity);



        //set params in animator
        animator.SetFloat(velocityXHash, velocityX);
        animator.SetFloat(velocityZHash, velocityZ);




        //Check if idle
        if (velocityZ == 0 && !animator.GetBool(isJumpingHash))
        {
            idle = true;
            animator.SetBool("idleInterrupt", false);
        }
        else
        {
            idle = false;
            animator.SetBool("idleInterrupt", true);
        }


        //Idle Counter
        if (idle)
        {
            idleTime += Time.deltaTime;

        }
        else
        {
            idleTime = 0;

        }

        //If idle for some time, will play other idle animation
        if (idleTime >= timeIdleUntilNewAnimation)
        {
            int randomAnimation = Random.Range(0, amountOfIdleAnimations);
            Debug.Log(randomAnimation);
            animator.SetInteger(idleAnimationHash,randomAnimation);
            animator.SetTrigger("idleAnimationTrigger");
            idleTime = 0;
        }

    }

    public void Jump()
    {
        
        animator.SetBool(isJumpingHash, true);
        sfx.PlayJumpSFX();
    }

    public void Land()
    {
        animator.SetBool(isJumpingHash, false);
    }

    public void Fall()
    {
        
        animator.SetBool("isFalling", true);
    }
    public void StopFall()
    {
        animator.SetBool("isFalling", false);
    }

}
