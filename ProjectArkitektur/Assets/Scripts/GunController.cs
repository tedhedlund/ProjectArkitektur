using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{

    Animator animator;

    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCamera;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Shoot");
            Shoot();
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
    }
}
