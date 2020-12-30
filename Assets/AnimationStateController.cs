using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    //Params
    [SerializeField] float accelerationX = 1;
    [SerializeField] float accelerationZ = 1;
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
        velocityXHash = Animator.StringToHash("velocityX");
        velocityZHash = Animator.StringToHash("velocityZ");
    }

    // Update is called once per frame
    void Update()
    {
        
        
        bool forwardPressed = Input.GetKey("w");
        bool leftPressed = Input.GetKey("a");
        bool rightPressed = Input.GetKey("d");
        bool runPressed = Input.GetKey("left shift");
        
        //Walking
        if (forwardPressed)
        {
            velocityZ += Time.deltaTime * accelerationZ;
            
            

        }
        if (!forwardPressed)
        {
            velocityZ -= Time.deltaTime * decceleration;
            
        }




       
        
        velocityX = Mathf.Clamp(velocityZ, 0, 1);
        animator.SetFloat(velocityXHash, velocityX);
    }
}
