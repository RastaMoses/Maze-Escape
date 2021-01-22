using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXControllerPlayer : MonoBehaviour
{
    
    
    [SerializeField] AudioSource audioSourceFootSteps;
    [SerializeField] CharacterController controller;
    [SerializeField] ThirdPersonMovement movement;
    [Header("Jump")]
    [SerializeField] AudioClip[] jumpSFXList;
    [SerializeField] [Range(0,1)] float jumpSFXVolume;
    [SerializeField] AudioClip[] jumpCrySFXList;
    [SerializeField] [Range(0, 1)] float jumpCrySFXVolume;
    [Header("Falling")]
    [SerializeField] AudioClip fallingClip;
    [SerializeField] [Range(0, 1)] float minFallVolume = 0.1f;
    [SerializeField] [Range(0, 1)] float maxFallVolume = 0.7f;
    [SerializeField] [Range(0,2)]float fallVolumeIncreaseSpeed = 1.5f;
    [SerializeField] AudioSource audioSourceFalling;
    [Header("Landing")]
    [SerializeField] AudioClip[] landingSFXList;
    [SerializeField] [Range(0, 1)] float landingSFXVolume;
    [SerializeField] AudioClip[] landingCrySFXList;
    [SerializeField] [Range(0, 1)] float landingCrySFXVolume;
    [SerializeField] AudioClip[] landrollSFXList;
    [SerializeField] [Range(0, 1)] float landrollSFXVolume;
    [SerializeField] AudioClip[] landrollCrySFXList;
    [SerializeField] [Range(0, 1)] float landrollCrySFXVolume;
    [Header("Footsteps")]
    [SerializeField] float walkStepVolume = 0.1f;
    [SerializeField] float runStepVolume = 0.3f;
    [SerializeField] AudioClip[] concrete, wood, dirt, metal, glass, sand, snow, stone, grass;



    //States
    bool alreadyFalling = false;
    float currentFallVolume;
    //Cached comp
    AudioSource audioSourcePlayer;
    private void Awake()
    {
        audioSourcePlayer = GetComponent<AudioSource>();
        
    }

    private void Update()
    {
        CheckFalling();
    }
    public void PlayLandingSFX()
    {
        
        if (landingCrySFXList.Length > 0)
        {
            int i = Random.Range(0, landingCrySFXList.Length - 1);
            audioSourcePlayer.PlayOneShot(landingCrySFXList[i], landingCrySFXVolume);
        }
        else
        {
            Debug.Log("No SFX in landingCrySFXList");
        }
        
        if (landingSFXList.Length > 0)
        {
            int e = Random.Range(0, landingSFXList.Length - 1);
            audioSourceFootSteps.PlayOneShot(landingSFXList[e], landingSFXVolume);
        }
        else
        {
            Debug.Log("No SFX in landingSFXList");
        }
        
        
    }

    public void PlayLandrollSFX()
    {
        
        if (landrollCrySFXList.Length > 0)
        {
            int i = Random.Range(0, landrollCrySFXList.Length - 1);
            audioSourcePlayer.PlayOneShot(landrollCrySFXList[i], landrollCrySFXVolume);
        }
        else
        {
            Debug.Log("No SFX in landrollCrySFXList");
        }
        
        if (landrollSFXList.Length > 0)
        {
            int e = Random.Range(0, landrollSFXList.Length - 1);
            audioSourceFootSteps.PlayOneShot(landrollSFXList[e], landrollSFXVolume);
        }
        else
        {
            Debug.Log("No SFX in landrollSFXList");
        }
    }
    public void PlayJumpSFX()
    {
        if (jumpCrySFXList.Length > 0)
        {
            int i = Random.Range(0, jumpCrySFXList.Length - 1);
            audioSourcePlayer.PlayOneShot(jumpCrySFXList[i], jumpCrySFXVolume);
        }
        else
        {
            Debug.Log("No SFX in jumpCrySFXList");
        }
        
        
        if (jumpSFXList.Length > 0)
        {
            int e = Random.Range(0, jumpSFXList.Length - 1);
            audioSourceFootSteps.PlayOneShot(jumpSFXList[e], jumpSFXVolume);
        }
        else
        {
            Debug.Log("No SFX in jumpSFXList");
        }
    }
    #region Falling
    
    void CheckFalling()
    {
        
        if (movement.GetIsFalling())
        {
            if (alreadyFalling)
            {
                IncreaseFallVolume();
                return;
            }
            else
            {
                alreadyFalling = true;
                audioSourceFalling.volume = minFallVolume;
                audioSourceFalling.clip = fallingClip;
                audioSourceFalling.Play();

                currentFallVolume = minFallVolume;
            }

        }
        else
        {
            audioSourceFalling.Stop();
            alreadyFalling = false;
        }
    }

    void IncreaseFallVolume()
    {
        
        currentFallVolume += fallVolumeIncreaseSpeed * Time.deltaTime;
        currentFallVolume = Mathf.Clamp(currentFallVolume, minFallVolume, maxFallVolume);
        audioSourceFalling.volume = currentFallVolume;
    }


    #endregion




    #region Footsteps
    public void FootStepWalk()
    {
        FootStep(false);
    }
    public void FootStepRun()
    {
        FootStep(true);
    }

    void FootStep(bool run)
    {

        string floortag = "Untagged";
        RaycastHit hit = new RaycastHit();
            

            
            
                if (Physics.Raycast(transform.position, Vector3.down, out hit, controller.bounds.extents.y + 0.5f))
                {
                    floortag = hit.collider.gameObject.tag;
                }
                if ((floortag == "Untagged" || floortag == "Concrete") && !run)
                {
                    WalkOnConcrete();
                }
                else if ((floortag == "Untagged" || floortag == "Concrete") && run)
                {
                    RunOnConcrete();
                }
                else if (floortag == "Wood" && !run)
                {
                    WalkOnWood();
                }
                else if (floortag == "Wood" && run)
                {
                    RunOnWood();
                }
                else if (floortag == "Dirt" && !run)
                {

                    WalkOnDirt();
                }
                else if (floortag == "Dirt" && run)
                {
                    RunOnDirt();
                }
                else if (floortag == "Metal" && !run)
                {

                    WalkOnMetal();
                }
                else if (floortag == "Metal" && run)
                {
                    RunOnMetal();
                }
                else if (floortag == "Glass" && !run)
                {
                    WalkOnGlass();
                }
                else if (floortag == "Glass" && run)
                {
                    RunOnGlass();
                }
                else if (floortag == "Sand" && !run)
                {
                    WalkOnSand();
                }
                else if (floortag == "Sand" && run)
                {
                    RunOnSand();
                }
                else if (floortag == "Snow" && !run)
                {
                    WalkOnSnow();
                }
                else if (floortag == "Snow" && run)
                {
                    RunOnSnow();
                }
                else if (floortag == "Stone" && !run)
                {
                    WalkOnStone();
                }
                else if (floortag == "Stone" && run)
                {
                    RunOnStone();
                }
                else if (floortag == "Grass" && !run)
                {
                    WalkOnGrass();
                }
                else if (floortag == "Grass" && run)
                {
                    RunOnGrass();

                }


                


         
        


    }


    


    /////////////////////////////////// CONCRETE //////////////////////////////////////// 
    void WalkOnConcrete() 
    { 

        audioSourceFootSteps.clip = concrete[Random.Range(0, concrete.Length)];
        audioSourceFootSteps.volume = 0.1f;
        audioSourceFootSteps.Play();

    }

    void RunOnConcrete()
    {

        audioSourceFootSteps.clip = concrete[Random.Range(0, concrete.Length)];
        audioSourceFootSteps.volume = 0.3f;
        audioSourceFootSteps.Play();

    }

////////////////////////////////// WOOD ///////////////////////////////////////////// 
    void WalkOnWood() 
    {

        audioSourceFootSteps.clip = wood[Random.Range(0, wood.Length)];
        audioSourceFootSteps.volume = 0.1f;
        audioSourceFootSteps.Play();
 
    }

    void RunOnWood()
    {

        audioSourceFootSteps.clip = wood[Random.Range(0, wood.Length)];
        audioSourceFootSteps.volume = 0.3f;
        audioSourceFootSteps.Play();

    }

/////////////////////////////////// DIRT //////////////////////////////////////////////
    void WalkOnDirt()
    {
        
        audioSourceFootSteps.clip = dirt[Random.Range(0, dirt.Length)];
        audioSourceFootSteps.volume = 0.1f;
        audioSourceFootSteps.Play();
 
    }

    void RunOnDirt()
    {
        
        audioSourceFootSteps.clip = dirt[Random.Range(0, dirt.Length)];
        audioSourceFootSteps.volume = 0.3f;
        audioSourceFootSteps.Play();

    }

////////////////////////////////// METAL /////////////////////////////////////////////// 
    void WalkOnMetal()
    {

        audioSourceFootSteps.clip = metal[Random.Range(0, metal.Length)];
        audioSourceFootSteps.volume = 0.1f;
        audioSourceFootSteps.Play();
 
    }

    void RunOnMetal()
    {

        audioSourceFootSteps.clip = metal[Random.Range(0, metal.Length)];
        audioSourceFootSteps.volume = 0.3f;
        audioSourceFootSteps.Play();

    }

////////////////////////////////// GLASS /////////////////////////////////////////////// 
    void WalkOnGlass()
    {

        audioSourceFootSteps.clip = glass[Random.Range(0, glass.Length)];
        audioSourceFootSteps.volume = 0.1f;
        audioSourceFootSteps.Play();
 
    }

    void RunOnGlass()
    {

        audioSourceFootSteps.clip = glass[Random.Range(0, glass.Length)];
        audioSourceFootSteps.volume = 0.3f;
        audioSourceFootSteps.Play();

    }

////////////////////////////////// SAND /////////////////////////////////////////////// 
    void WalkOnSand()
    {

        audioSourceFootSteps.clip = sand[Random.Range(0, sand.Length)];
        audioSourceFootSteps.volume = 0.1f;
        audioSourceFootSteps.Play();
 
    }

    void RunOnSand()
    {

        audioSourceFootSteps.clip = sand[Random.Range(0, sand.Length)];
        audioSourceFootSteps.volume = 0.3f;
        audioSourceFootSteps.Play();

    }

////////////////////////////////// SNOW /////////////////////////////////////////////// 
    void WalkOnSnow()
    {

        audioSourceFootSteps.clip = snow[Random.Range(0, snow.Length)];
        audioSourceFootSteps.volume = 0.1f;
        audioSourceFootSteps.Play();
 
    }

    void RunOnSnow()
    {

        audioSourceFootSteps.clip = snow[Random.Range(0, snow.Length)];
        audioSourceFootSteps.volume = 0.3f;
        audioSourceFootSteps.Play();

    }

////////////////////////////////// STONE /////////////////////////////////////////////// 
    void WalkOnStone()
    {

        audioSourceFootSteps.clip = stone[Random.Range(0, stone.Length)];
        audioSourceFootSteps.volume = 0.1f;
        audioSourceFootSteps.Play();
 
    }

    void RunOnStone()
    {

        audioSourceFootSteps.clip = stone[Random.Range(0, stone.Length)];
        audioSourceFootSteps.volume = 0.3f;
        audioSourceFootSteps.Play();

    }

////////////////////////////////// GRASS /////////////////////////////////////////////// 
    void WalkOnGrass()
    {

        audioSourceFootSteps.clip = grass[Random.Range(0, grass.Length)];
        audioSourceFootSteps.volume = 0.1f;
        audioSourceFootSteps.Play();
 
    }

    void RunOnGrass()
    {

        audioSourceFootSteps.clip = grass[Random.Range(0, grass.Length)];
        audioSourceFootSteps.volume = 0.3f;
        audioSourceFootSteps.Play();

    }
    
    #endregion
}
