using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    //Serialized Variables
    [Header("General")]
    [SerializeField] float maxHP = 100;

    [Header("Fire")]
    [SerializeField] float fireDamage = 10;
    [SerializeField] float fireDamageInterval;


    //States
    bool onFire;
    bool onFirecoroutineActive;


    float currentHP;
    IEnumerator onFireCoroutine;
    void Start()
    {

        currentHP = maxHP;

        //save coroutines in variables
        onFireCoroutine = OnFire();
        onFirecoroutineActive = false;
    }

    void Update()
    {
    }

    public void SetOnFire()
    {
        onFire = true;

        if (!onFirecoroutineActive)
        {
            StartCoroutine(onFireCoroutine);
        }
    }

    public void RemoveOnFire()
    {
        onFire = false;
        StopCoroutine(onFireCoroutine);
        onFirecoroutineActive = false;
    }

    IEnumerator OnFire()
    {
        onFirecoroutineActive = true;
        while (onFire)
        {
            yield return new WaitForSeconds(fireDamageInterval);
            currentHP -= fireDamage;
            
        }
    }
}
