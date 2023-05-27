using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class RestartScript : MonoBehaviour
{

    [SerializeField] Player_Controller pController;
    float timer = 0f;

    // Update is called once per frame
    void Update()
    {

        if(pController.playerAlive)
        {
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }                
        }
        else
        {
            timer += Time.deltaTime;
           
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
            if (timer >= 4f)
            {
                if (Input.anyKey)
                {
                    SceneManager.LoadScene(0);
                }
            }          
        }
    }
}
