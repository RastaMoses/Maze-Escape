using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopUp : MonoBehaviour
{
    [SerializeField] TextMeshPro textPrefab;
    public void PopUp(int damage)
    {
        TextMeshPro damagePopUp = Instantiate(textPrefab, transform.position, Quaternion.identity);
        damagePopUp.gameObject.GetComponent<DamagePopUpMover>().Setup(damage);
    }
}
