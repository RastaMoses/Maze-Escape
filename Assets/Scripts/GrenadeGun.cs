using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeGun : MonoBehaviour
{
    [SerializeField] Grenade grenadePrefab;
    [SerializeField] Transform gun;
    [SerializeField] float grenadeCoolDown = 2f;
    [SerializeField] float throwForce = 40;
    [SerializeField] bool impactGrenade;

    //States
    [SerializeField] bool onCooldown = false; //Debug



    IEnumerator cooldownCoroutine;
    private void Start()
    {
    }
    void Update()
    {
        if (!onCooldown)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                LaunchGrenade();
                ActivateCooldown();
            }
        }
        
    }

    private void LaunchGrenade()
    {
        Grenade grenade = Instantiate(grenadePrefab, gun.position, gun.rotation);
        grenade.GetComponent<Rigidbody>().AddForce(gun.forward * throwForce, ForceMode.Impulse);
        if (impactGrenade) // if impact will setup on grenade
        {
            grenade.SetImpactGrenade();
        }
    }



    void ActivateCooldown()
    {
        onCooldown = true;
        StartCoroutine("GrenadeCooldown");
    }

    IEnumerator GrenadeCooldown()
    {
        yield return new WaitForSeconds(grenadeCoolDown);
        onCooldown = false;
    }
}
