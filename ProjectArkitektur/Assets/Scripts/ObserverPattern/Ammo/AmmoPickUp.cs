using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{
    [SerializeField] private EventSO ammoEvent;
    [SerializeField] private Vector3 distance;

    private GameObject player;
    private GameObject ammoBox;


    void Start()
    {
        ammoBox = this.gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        distance = gameObject.transform.position - player.transform.position;
        if ( Vector3.Distance(player.transform.position, gameObject.transform.position) <= 2)
        {
            Debug.Log("Colliding with Player");
            ammoEvent.InvokeAmmo();                 // Sending an invoke to all subscribers
            Destroy(gameObject);
        }
    }
}
