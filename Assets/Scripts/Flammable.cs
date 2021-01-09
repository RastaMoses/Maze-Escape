using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flammable : MonoBehaviour
{
    [SerializeField] bool burnsDown;
    [SerializeField] float timeToBurnDown = 20;
    //States
    bool isBurning;

    GameObject fireChildObject;

    private void Start()
    {
        isBurning = false;
        fireChildObject = gameObject.transform.GetChild(0).gameObject;
    }
    void Update()
    {
        
    }


     public void SetBurning()
    {
        if (!isBurning)
        {
            fireChildObject.SetActive(true);
            isBurning = true;
            if (burnsDown)
            {
                StartCoroutine("BurnDown");
            }
            
        }
    }

    public void RemoveBurning()
    {
        if (isBurning)
        {
            fireChildObject.SetActive(false);
            isBurning = false;
            if (burnsDown)
            {
                StopCoroutine("BurnDown");
            }
        }
    }


    IEnumerator BurnDown()
    {
        yield return new WaitForSeconds(timeToBurnDown);
        RemoveBurning();
        StartCoroutine("DestroySelf");
        
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(0);
        Destroy(gameObject);
    }
    
}
