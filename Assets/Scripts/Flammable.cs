using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flammable : MonoBehaviour
{
    [SerializeField] float timeToBurnDown = 5;
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
            StartCoroutine("BurnDown");
        }
    }

    public void RemoveBurning()
    {
        if (isBurning)
        {
            fireChildObject.SetActive(false);
            isBurning = false;
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
