using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{

    private Transform cameraTransfrom;

    void Start()
    {
        cameraTransfrom = GameObject.Find("Main Camera").transform;
    }

    
    void Update()
    {
        
    }
}
