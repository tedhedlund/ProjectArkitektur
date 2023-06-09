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
    public enum CurrentGun { pistol, AR };
    public CurrentGun currentGun;

    bool ammoEmpty = false;
    int currentAmmo;
    const int maxAmmo = 30;

    float fireRate = 0.06f;
    float nextFire;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
        Reload();
           
    }

    void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.SetTrigger("Reload");
            currentAmmo = maxAmmo;
            ammoEmpty = false;
        }   
    }

    void Shoot()
    {
        if (!ammoEmpty)
        {
            if (Input.GetButton("Fire1"))
            {

                animator.SetBool("IsFiring", true);

                nextFire += Time.deltaTime;

                if (nextFire >= fireRate)
                {
                    ShootRayCast();
                    currentAmmo--;
                    nextFire = 0;
                }

                if (currentAmmo <= 0)
                {
                    animator.SetBool("IsFiring", false);
                    ammoEmpty = true;
                }

            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("IsFiring", false);
        }
    }

    void ShootRayCast()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hitInfo, range))
        {
            Debug.Log(hitInfo.transform.name);
        }

        //Debug.Log($"Ammo left: {currentAmmo}");
    }
}
