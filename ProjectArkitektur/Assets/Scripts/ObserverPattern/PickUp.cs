using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private IntEventSO ammoEvent;
    [SerializeField] private int ammoValue = 30;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Colliding with Player");
            ammoEvent.Invoke(ammoValue);
            Destroy(gameObject);
        }
    }
}
