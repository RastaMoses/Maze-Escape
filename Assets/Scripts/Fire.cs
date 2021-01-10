using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] bool extinguishAfterTime;
    [SerializeField] float timeToExtinguish;
    List<Collider> healthEntitiesInFire;
    List<Collider> flammableEntitiesInFire;
    private void Start()
    {
        
        healthEntitiesInFire = new List<Collider>();
        flammableEntitiesInFire = new List<Collider>();
        if (extinguishAfterTime)
        {
            StartCoroutine("ExtinguishAfterTime");
        }
    }

    //called when enabled
    private void OnEnable()
    {
        
        healthEntitiesInFire = new List<Collider>();
        if (extinguishAfterTime)
        {
            StartCoroutine("ExtinguishAfterTime");
        }

    }

    private void OnDisable()
    {
        foreach (Collider entity in healthEntitiesInFire)
        {
            entity.gameObject.GetComponent<Health>().RemoveOnFire();
        }

        if (flammableEntitiesInFire.Count != 0)
        {
            foreach (Collider entity in flammableEntitiesInFire)
            {
                if (entity)
                {
                    entity.gameObject.GetComponent<Flammable>().RemoveFireColliderFromList(gameObject.GetComponent<Collider>());
                }

            }
        }
        if (extinguishAfterTime)
        {
            StopCoroutine("ExtinguishAfterTime");
        }
    }
    

    
    void Extinguish()
    {

        Debug.Log("Extinguish");
        if (GetComponentInParent<Flammable>())
        {
            GetComponentInParent<Flammable>().RemoveBurning();
        }
        else
        {
            gameObject.SetActive(false);
        }
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
            if (!healthEntitiesInFire.Contains(other))
            {
                healthEntitiesInFire.Add(other);
            }
        }
        if (other.gameObject.GetComponent<Flammable>())
        {
            if (!flammableEntitiesInFire.Contains(other))
            {
                flammableEntitiesInFire.Add(other);
            }

            other.gameObject.GetComponent<Flammable>().AddFireColliderToList(this.gameObject.GetComponent<Collider>());
            other.gameObject.GetComponent<Flammable>().SetBurning();
        }

    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.GetComponent<Health>())
        {
            var entityHealth = other.gameObject.GetComponent<Health>();
            entityHealth.RemoveOnFire();
            if (healthEntitiesInFire.Contains(other))
            {
                healthEntitiesInFire.Remove(other);
            }

        }
        if (other.gameObject.GetComponent<Flammable>())
        {
            if (flammableEntitiesInFire.Contains(other))
            {
                flammableEntitiesInFire.Remove(other);
            }

            other.gameObject.GetComponent<Flammable>().RemoveFireColliderFromList(this.gameObject.GetComponent<Collider>());
        }
    }
    
}
