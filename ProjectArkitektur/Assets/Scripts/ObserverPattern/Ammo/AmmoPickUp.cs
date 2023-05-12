using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{
    [SerializeField] private IntEventSO ammoEvent;
    [SerializeField] private int ammoValue = 30;
    [SerializeField] private Vector3 distance;

    private GameObject player;
    private GameObject ammoBox;

    // pickUp.AmmoSpawn(agent.transform.position); //Sätta in i ammoManager?

    //private void OnTriggerEnter(Transform other)
    //{
    //    if (1 >= Vector3.Distance(other.transform.position, collider.transform.position)) 
    //    {
    //        Debug.Log("Colliding with Player");
    //        ammoEvent.Invoke(ammoValue);
    //        Destroy(gameObject);
    //    }


    //}

    void Start()
    {
        ammoBox = this.gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (2 >= Vector3.Distance(gameObject.transform.position, player.transform.position))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("E pressed");
                ammoEvent.InvokeAmmo(ammoValue);
                Destroy(gameObject);
            }
        }
        distance = gameObject.transform.position - player.transform.position;
        if ( Vector3.Distance(player.transform.position, gameObject.transform.position) <= 2)
        {
            Debug.Log("Colliding with Player");
            ammoEvent.InvokeAmmo(ammoValue);
            Destroy(gameObject);
        }

    }

    public void AmmoSpawn(Vector3 position)
    {
        Quaternion rotation = new Quaternion(0, 0, 0, 0);

        Instantiate(ammoBox, position, rotation);
    }
}
