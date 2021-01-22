using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electric : MonoBehaviour
{
    [SerializeField] GameObject electricParticlePrefab;
    [SerializeField] Transform particlePosition;
    [SerializeField] float radiusToChain = 3f;
    [SerializeField] float timeUntilChain = 2f;
    [SerializeField] bool activateOnStart = false;
    [Header("Timer")]
    [SerializeField] bool deactivateAfterTime = true;
    [SerializeField] float timeActive = 8f;
    [SerializeField] float timeUntilReactivateable = 12f;
    
    //States
    bool active = false;
    bool onTimer = false;
    bool canChain = false;

    GameObject electricParticle;

    public void Start()
    {
        if (activateOnStart)
        {
            Activate();
        }
        
    }


    public void Activate()
    {
        if (!active && !onTimer)
        {
            active = true;
            SpawnParticles();
            StartCoroutine(WaitTimeUntilChain());
            if (deactivateAfterTime)
            {
                onTimer = true;
                StartCoroutine(ActiveTimer());
                StartCoroutine(ReactivateTimer());
            }
            
        }
    }

    public void Deactivate()
    {

        DespawnParticles();
        active = false;
        canChain = false;
    }

    private void Update()
    {
        if (active && canChain)
        {
            ElectrifyChain();
        }
    }

    void SpawnParticles()
    {
        electricParticle = Instantiate(electricParticlePrefab, particlePosition.position, Quaternion.identity);
    }

    void DespawnParticles()
    {
        Destroy(electricParticle);
    }
    

    IEnumerator WaitTimeUntilChain()
    {
        yield return new WaitForSeconds(timeUntilChain);
        canChain = true;
    }
    void ElectrifyChain()
    {
        Collider[] hitColliders = Physics.OverlapSphere(particlePosition.position, radiusToChain);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.GetComponent<Electric>())
            {
                hitCollider.gameObject.GetComponent<Electric>().Activate();
            }
            if (hitCollider.gameObject.GetComponent<Health>())
            {
                hitCollider.gameObject.GetComponent<Health>().SetElectric();
            }

        }
    }


    public bool GetActive()
    {
        return active;
    }


    IEnumerator ActiveTimer()
    {
        yield return new WaitForSeconds(timeActive);
        Deactivate();
    }
    IEnumerator ReactivateTimer()
    {

        yield return new WaitForSeconds(timeUntilReactivateable);
        onTimer = false;
    }
}
