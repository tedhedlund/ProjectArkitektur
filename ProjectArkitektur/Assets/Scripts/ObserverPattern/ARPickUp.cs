using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARPickUp : MonoBehaviour
{
    [SerializeField] private BoolEvent BuyAREvent;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 distance;
    [SerializeField] public int boughtValue = 1;

    private void Update()
    {
        if (2 >= Vector3.Distance(gameObject.transform.position, player.transform.position) /*&& player.money >= 100*/ && Input.GetKeyDown(KeyCode.E))
        {

            //if (Input.GetKeyDown(KeyCode.Q))
            //{
            Debug.Log("E pressed");
            BuyAREvent.InvokeARBuy(boughtValue);
            //Destroy(gameObject);
            //}
        }
        distance = gameObject.transform.position - player.transform.position;

    }
}
