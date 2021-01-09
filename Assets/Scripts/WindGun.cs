using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindGun : MonoBehaviour
{
    [Header("Explosion")]
    [SerializeField] float radius;
    [SerializeField] float power;

    List<Collider> triggerList = new List<Collider>();
    // Update is called once per frame
    

    private void OnMouseDown()
    {
        Explode();

    }
    
    void Explode()
    {
        Debug.Log("EXPLOSION");
        Vector3 explosionPosition = GetComponentInParent<Transform>().position;
        foreach (Collider hit in triggerList)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            rb.AddExplosionForce(power, explosionPosition, radius);

        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null && !triggerList.Contains(other))
        {
            triggerList.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Rigidbody>() != null && triggerList.Contains(other))
        {
            triggerList.Remove(other);
        }
    }
}
