using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ARGunControl : MonoBehaviour
{
    Animator animator;

    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCamera;

    bool ammoEmpty;
    int maxAmmo = 30;
    int shotsFired;

    float shootTimeInterval = 0.5f;
    float nextTimeToFire = 0f;
    float counter = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        while (Input.GetMouseButtonDown(0))
        {

            if (ammoEmpty)
            {
                // Do nothing
            }

            animator.SetBool("IsFiring", true);

            //Shoot();
            //shotsFired++;

            counter += Time.deltaTime;

            if (counter >= shootTimeInterval)
            {
                Shoot();
                counter = 0;
                shotsFired++;
            }
            else if (shotsFired >= 30)
            {
                ammoEmpty = true;
            }

            

        }
        if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("IsFiring", false);
            Debug.Log("LMB up");
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger("Reload");
        }

        Debug.Log($"Shots fired: {shotsFired}");
    }

    void Shoot()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hitInfo, range))
        {
            Debug.Log(hitInfo.transform.name);
        }
    }
}
