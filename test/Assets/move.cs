using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    public CharacterController player;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float inputHorizontal = Input.GetAxis("Horizontal") * 10 * Time.deltaTime;
        float inputVertical = Input.GetAxis("Vertical") * 10 * Time.deltaTime;

        Vector3 dir = inputHorizontal * transform.right + inputVertical * transform.forward;

        player.Move(dir * Time.deltaTime);   
    }
}
