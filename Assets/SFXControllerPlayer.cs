using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXControllerPlayer : MonoBehaviour
{
    [SerializeField] AudioClip[] jumpSFXList;
    [SerializeField] AudioClip walkSFX;
    [SerializeField] AudioClip runSFX;


    //Cached comp
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayJumpSFX()
    {
        int i = Random.Range(0, jumpSFXList.Length);
        audioSource.PlayOneShot(jumpSFXList[i - 1]);
    }
   
}
