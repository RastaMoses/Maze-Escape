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
    List<Collider> fireColliderList;

    private void Start()
    {
        fireColliderList = new List<Collider>();
        isBurning = false;
        fireChildObject = gameObject.transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        Debug.Log(fireColliderList.Count);
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
            isBurning = false;
            if (fireColliderList.Count == 1) //if there is no other fire around, stops burning if fire is extinguished
            {
                fireChildObject.SetActive(false);
                if (burnsDown) //Burn Down Coroutine is reset to 0
                {
                    StopCoroutine("BurnDown");
                }
            }
            // ExtinguishTimer Restart
            else
            {
                Debug.Log("Restart Extinguish Timer");
                fireChildObject.GetComponent<Fire>().ResetExtinguishTimer();
                isBurning = true;
            }
            
        }
    }


    IEnumerator BurnDown()
    {
        //Destroys gameObject after certain time of constant burning
        yield return new WaitForSeconds(timeToBurnDown);
        RemoveBurning();
        StartCoroutine("DestroySelf");
    }

    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(0);
        Destroy(gameObject);
    }

    //Need list to safe all active fires touching object
    public void RemoveFireColliderFromList(Collider collider)
    {
        if (fireColliderList.Contains(collider))
        {
            fireColliderList.Remove(collider);
        }
    }
    public void AddFireColliderToList(Collider collider)
    {
        if (!fireColliderList.Contains(collider))
        {
            fireColliderList.Add(collider);
        }
    }
}
