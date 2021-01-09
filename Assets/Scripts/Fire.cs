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
        StartCoroutine("Extinguish");
    }
    void Update()
    {
        
        
    }


    IEnumerator Extinguish()
    {
        yield return new WaitForSeconds(timeToExtinguish);
        foreach (Collider entity in entitiesInFire)
        {
            entity.gameObject.GetComponent<Health>().RemoveOnFire();
        }
        
    }

     public void ResetExtinguishTimer()
    {
        StopCoroutine("Extinguish");
        StartCoroutine("Extinguish");
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
