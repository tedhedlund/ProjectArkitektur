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

    bool ammoEmpty = false;
    int maxAmmo = 30;
    int shotsFired;

    float fireRate = 0.05f;
    float nextFire;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ammoEmpty)
        {
           

            if (Input.GetButton("Fire1"))
            {



                animator.SetBool("IsFiring", true);

                nextFire += Time.deltaTime;

                if (nextFire >= fireRate && !ammoEmpty)
                {
                    Shoot();
                    shotsFired++;
                    nextFire = 0;
                }

                //if (Time.time > nextFire)
                //{
                //    nextFire = Time.time + fireRate;
                //    Shoot();
                //}

                if (shotsFired >= 30)
                {
                    ammoEmpty = true;
                }



            }
        }

        
        if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("IsFiring", false);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger("Reload");
        }

       
    }

    void Shoot()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hitInfo, range))
        {
            Debug.Log(hitInfo.transform.name);
        }

        Debug.Log($"Shots fired: {shotsFired}");
    }
}
