using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopUpMover : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1;

    TextMeshPro textMesh;
    // Start is called before the first frame update
    void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup (int damage)
    {
        textMesh.SetText(damage.ToString());
    }
    private void Update()
    {
        transform.position +=new Vector3(0, Time.deltaTime * moveSpeed,0);
    }
}
