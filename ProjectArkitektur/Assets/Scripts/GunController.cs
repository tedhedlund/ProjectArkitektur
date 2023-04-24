using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCamera;

    Animator animator;

    bool ammoEmpty = false;
    int currentAmmo;
    const int maxAmmo = 15;

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
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger("Shoot");
                ShootRayCast();
                currentAmmo--;
            }
            else if (currentAmmo <= 0)
            {
                ammoEmpty = true;
            }
        }   
    }

    void ShootRayCast()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hitInfo, range))
        {
            Debug.Log(hitInfo.transform.name);
        }
    }
}
