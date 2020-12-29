using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    int velocityHash;
    int isRunningHash;
    float velocity = 0f;
    [SerializeField] float acceleration = 1;
    [SerializeField] float sprintAcceleration = 0.6f;
    [SerializeField] float decceleration = 1;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        velocityHash = Animator.StringToHash("velocity");
        isRunningHash = Animator.StringToHash("isRunning");
    }

    // Update is called once per frame
    void Update()
    {
        
        
        bool forwardPressed = Input.GetKey("w");
        bool runPressed = Input.GetKey("left shift");
        
        //Walking
        if (forwardPressed)
        {
            velocity += Time.deltaTime * acceleration;
            
            

        }
        if (!forwardPressed)
        {
            velocity -= Time.deltaTime * decceleration;
            
        }




       
        
        velocity = Mathf.Clamp(velocity, 0, 1);
        animator.SetFloat(velocityHash, velocity);
    }
}
