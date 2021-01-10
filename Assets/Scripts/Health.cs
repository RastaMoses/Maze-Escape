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
    [SerializeField] bool onFirecoroutineActive; //for Debug serialized

    [SerializeField] float currentHP; //Serialized for Debug


    List<Collider> fireColliderList;


    //Cached Component Reference
    IEnumerator onFireCoroutine;




    void Start()
    {

        currentHP = maxHP;

        //save coroutines in variables
        onFireCoroutine = OnFire();
        onFirecoroutineActive = false;
        fireColliderList = new List<Collider>();
    }

    void Update()
    {
        CheckHP();
    }

    public void SetOnFire()
    {
        //Sets onfire, if coroutine not already active
        onFire = true;

        if (!onFirecoroutineActive)
        {

            onFirecoroutineActive = true;
            StartCoroutine(onFireCoroutine);
        }
    }

    public void RemoveOnFire()
    {
        //if there is no fire, will stop burning coroutine
        if (fireColliderList.Count == 0)
        {
            onFire = false;
            StopCoroutine(onFireCoroutine);
            onFirecoroutineActive = false;
        }

    }

    IEnumerator OnFire() //Deals damage while on fire
    {
        while (onFire)
        {
            yield return new WaitForSeconds(fireDamageInterval);
            TakeDamage(fireDamage);
            
        }
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        DamagePopUp(damage); //Test
    }

    void CheckHP()
    {
        //Checks if dead
        if (currentHP <= 0)
        {
            Debug.Log(gameObject.name + (" Dead"));
        }
    }

    public void AddToFireList(Collider fire)
    {
        //Adds if not already to all active fires list
        if (!fireColliderList.Contains(fire))
        {
            fireColliderList.Add(fire);
        }
    }

    public void RemoveFromFireList(Collider fire)
    {
        //Removes if there from active fires list
        if (fireColliderList.Contains(fire))
        {
            fireColliderList.Remove(fire);
        }
        
    }





    //Testing
    void DamagePopUp(float damage)
    {
        if (GetComponent<DamagePopUp>())
        {
            GetComponent<DamagePopUp>().PopUp((int)damage);
        }
    }
}
