using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] float damage = 10;
    [SerializeField] float explosionDelay = 3f;
    [SerializeField] float explosionRadius = 15f;
    [SerializeField] float explosionForce = 20f;



    //States
    bool explodeOnImpact = false;

    void Start()
    {
        if (!explodeOnImpact)
        {
            StartCoroutine(Timer());
        }
    }

      IEnumerator Timer()
    {
        yield return new WaitForSeconds(explosionDelay);
        Explode();
    }

    void Explode()
    {
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliderArray)
        {
            if (collider.gameObject.GetComponent<Health>())
            {
                collider.gameObject.GetComponent<Health>().TakeDamage(damage);
            }
            if (collider.gameObject.GetComponent<Rigidbody>())
            {
                collider.gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
        Destroy(gameObject);
    }

    public void SetImpactGrenade()
    {
        explodeOnImpact = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (explodeOnImpact)
        {
            Explode();
        }
    }

}
