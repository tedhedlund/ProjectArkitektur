using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class SettingsScript : MonoBehaviour
{

   public bool activeSettings = false;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!activeSettings) 
                activeSettings = true;
            else if(activeSettings)
                activeSettings = false;
        }

        if(activeSettings)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    //Method only for the back button
    public void Deactivate()
    {
        activeSettings = false;
    }
}
