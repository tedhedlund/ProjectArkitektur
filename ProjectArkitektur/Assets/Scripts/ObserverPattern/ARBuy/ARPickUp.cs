using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARPickUp : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 distance;
    [SerializeField] public int boughtValue = 1;

    private void Update()
    {
        if (2 >= Vector3.Distance(gameObject.transform.position, player.transform.position) /*&& player.money >= 100*/ && Input.GetKeyDown(KeyCode.E))
        {

            Debug.Log("E pressed");
            
            boughtValue++;
            
        }
        distance = gameObject.transform.position - player.transform.position;

    }
}
