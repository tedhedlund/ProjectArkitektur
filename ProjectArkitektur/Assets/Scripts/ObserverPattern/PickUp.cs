using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] private IntEventSO ammoEvent;
    [SerializeField] private int ammoValue = 30;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 distance;

    private void Start()
    {
        
    }

    //private void OnTriggerEnter(Transform other)
    //{
    //    if (1 >= Vector3.Distance(other.transform.position, collider.transform.position)) 
    //    {
    //        Debug.Log("Colliding with Player");
    //        ammoEvent.Invoke(ammoValue);
    //        Destroy(gameObject);
    //    }

        
    //}

    private void Update()
    {
        if (2 >= Vector3.Distance(gameObject.transform.position, player.transform.position))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("E pressed");
                ammoEvent.Invoke(ammoValue);
                Destroy(gameObject);
            }
        }
        distance = gameObject.transform.position - player.transform.position;
        if ( Vector3.Distance(player.transform.position, gameObject.transform.position) <= 2)
        {
            Debug.Log("Colliding with Player");
            ammoEvent.Invoke(ammoValue);
            Destroy(gameObject);
        }

    }
}
