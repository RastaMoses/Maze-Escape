using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] float timeToExtinguish;

    List<Collider> entitiesInFire;
    private void Start()
    {
        
        entitiesInFire = new List<Collider>();
        StartCoroutine("ExtinguishAfterTime");
    }

    //called when enabled
    private void OnEnable()
    {
        
        entitiesInFire = new List<Collider>();
        StartCoroutine("ExtinguishAfterTime");
    }

    private void OnDisable()
    {
        StopCoroutine("ExtinguishAfterTime");
    }
    

    
    void Extinguish()
    {
        foreach (Collider entity in entitiesInFire)
        {
            entity.gameObject.GetComponent<Health>().RemoveOnFire();
        }
        Destroy(gameObject);
    }

    IEnumerator ExtinguishAfterTime()
    {
        yield return new WaitForSeconds(timeToExtinguish);
        Extinguish();
        
    }

     public void ResetExtinguishTimer()
    {
        StopCoroutine("ExtinguishAfterTime");
        StartCoroutine("ExtinguishAfterTime");
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
