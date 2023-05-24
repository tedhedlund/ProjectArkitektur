using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARPickUp : MonoBehaviour
{

    private GameObject player;
    private Vector3 distance;
    public int boughtValue = 1;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (2 >= Vector3.Distance(this.transform.position, player.transform.position) /*&& player.money >= 100*/ && Input.GetKeyDown(KeyCode.E))
        {

            Debug.Log("E pressed");
            
            boughtValue++;
            Destroy(this);
            
        }
        distance = gameObject.transform.position - player.transform.position;

    }
}
