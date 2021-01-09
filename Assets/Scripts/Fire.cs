using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] bool extinguishAfterTime;
    [SerializeField] float timeToExtinguish;
    List<Collider> entitiesInFire;
    private void Start()
    {
        
        entitiesInFire = new List<Collider>();
        if (extinguishAfterTime)
        {
            StartCoroutine("ExtinguishAfterTime");
        }
    }

    //called when enabled
    private void OnEnable()
    {
        
        entitiesInFire = new List<Collider>();
        if (extinguishAfterTime)
        {
            StartCoroutine("ExtinguishAfterTime");
        }
    }

    private void OnDisable()
    {
        if (extinguishAfterTime)
        {
            StopCoroutine("ExtinguishAfterTime");
        }
    }
    

    
    void Extinguish()
    {
        foreach (Collider entity in entitiesInFire)
        {
            entity.gameObject.GetComponent<Health>().RemoveOnFire();
        }
        gameObject.SetActive(false);
    }

    IEnumerator ExtinguishAfterTime()
    {
        yield return new WaitForSeconds(timeToExtinguish);
        Extinguish();
        
    }

     public void ResetExtinguishTimer()
    {
        if (extinguishAfterTime)
        {
            StopCoroutine("ExtinguishAfterTime");
            StartCoroutine("ExtinguishAfterTime");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Health>() != null)
        {
            var entityHealth = other.gameObject.GetComponent<Health>();
            entityHealth.SetOnFire();
            if (!entitiesInFire.Contains(other))
            {
                entitiesInFire.Add(other);
            }
        }
        if (other.gameObject.GetComponent<Flammable>() != null)
        {
            other.gameObject.GetComponent<Flammable>().SetBurning();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<Health>() != null)
        {
            var entityHealth = other.gameObject.GetComponent<Health>();
            entityHealth.RemoveOnFire();
            if (entitiesInFire.Contains(other))
            {
                entitiesInFire.Remove(other);
            }

        }
    }
    
}
