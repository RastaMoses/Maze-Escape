using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindGun : MonoBehaviour
{
    [Header("Explosion")]
    [SerializeField] float radius;
    [SerializeField] float power;
    [SerializeField] float upwardsPower;

    List<Collider> triggerList = new List<Collider>();
    // Update is called once per frame
    

    private void OnMouseDown()
    {
        Explode();

    }
    
    void Explode()
    {
        Vector3 explosionPosition = GetComponentInParent<Transform>().position; //Determin position where explosion comes from (player)
        foreach (Collider hit in triggerList)
        {
            
            if (hit) //if collider hasnt been destroyed while inside trigger zone
            {
                //Add explosionforce to all rigidbodies in zone
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                rb.AddExplosionForce(power, explosionPosition, radius, upwardsPower);
            }
        }

    }

    //Add entity to collider list, if enters zone and has rigidbody
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null && !triggerList.Contains(other))
        {
            triggerList.Add(other);
        }
    }

    //Remove from affected collider list if exits zone
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null && triggerList.Contains(other))
        {
            triggerList.Remove(other);
        }
    }
}
