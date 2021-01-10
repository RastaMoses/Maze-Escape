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
            //Removes burned from health entities when being disabled and removes self from their active fire list
            if (entity)
            {
                entity.gameObject.GetComponent<Health>().RemoveOnFire();
                entity.gameObject.GetComponent<Health>().RemoveFromFireList(this.gameObject.GetComponent<Collider>());

            }
        }

        if (flammableEntitiesInFire.Count != 0)
        {
            //removes self from flammable object fire list
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
        //If has Flammable Object as parent starts remove burning, otherwise disables self
        if (GetComponentInParent<Flammable>())
        {
            GetComponentInParent<Flammable>().RemoveBurning();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    //Timer for extinguish self
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
        //if there is health component on collided entity will save to list and set on fire
        if (other.gameObject.GetComponent<Health>() != null)
        {
            var entityHealth = other.gameObject.GetComponent<Health>();
            entityHealth.SetOnFire();
            if (!healthEntitiesInFire.Contains(other))
            {
                healthEntitiesInFire.Add(other);
            }
            //Adds self to others list
            entityHealth.AddToFireList(this.gameObject.GetComponent<Collider>());
        }
        //if flammable collider, then saves self to its list and it to own list and sets burning
        if (other.gameObject.GetComponent<Flammable>())
        {
            var entityFlammable = other.gameObject.GetComponent<Flammable>();
            if (!flammableEntitiesInFire.Contains(other))
            {
                flammableEntitiesInFire.Add(other);
            }
            //Adds self to others list and sets burning
            entityFlammable.AddFireColliderToList(this.gameObject.GetComponent<Collider>());
            entityFlammable.SetBurning();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        //if health object collided saves to list and removes fire
        if (other.gameObject.GetComponent<Health>())
        {
            var entityHealth = other.gameObject.GetComponent<Health>();
            
            if (healthEntitiesInFire.Contains(other))
            {
                healthEntitiesInFire.Remove(other);
            }
            //Adds self to others list
            entityHealth.RemoveFromFireList(this.gameObject.GetComponent<Collider>());
            entityHealth.RemoveOnFire();

        }
        // if flammable object collided will remove from list and self from its list
        if (other.gameObject.GetComponent<Flammable>())
        {
            var entityFlammable = other.gameObject.GetComponent<Flammable>();
            if (flammableEntitiesInFire.Contains(other))
            {
                flammableEntitiesInFire.Remove(other);
            }
            //Adds self to others list
            entityFlammable.RemoveFireColliderFromList(this.gameObject.GetComponent<Collider>());
        }
    }
    
}
