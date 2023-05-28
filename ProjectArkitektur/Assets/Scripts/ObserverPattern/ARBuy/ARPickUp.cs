using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARPickUp : MonoBehaviour
{

    private GameObject player, ARgun;
    private Vector3 distance;
    public bool arPickedUp = false;

    

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ARgun = GameObject.FindGameObjectWithTag("ARPickup");
        
    }

    private void Update()
    {
        if (3 >= Vector3.Distance(this.transform.position, player.transform.position))
        {

            
            arPickedUp = true;
            Destroy(ARgun);
            
        }
        distance = gameObject.transform.position - player.transform.position;

    }
}
