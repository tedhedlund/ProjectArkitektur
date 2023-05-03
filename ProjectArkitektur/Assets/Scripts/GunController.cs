using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private int maxAmmo = 30;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float fireRate = 0.06f;
    [SerializeField] private float hipfireRecoil;
    [SerializeField] private float adsRecoil;
    [SerializeField] private float adsInSpeed;
    [SerializeField] private float adsOutSpeed;
    [SerializeField] private BulletHoles bulletHoles;
    [SerializeField] private Player_Look camera;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private Player_Controller player;
    public enum CurrentGun { pistol, AR };
    public CurrentGun currentGun;

    
    private Animator animator;
    private LayerMask ignoreRaycast;

    public Vector3 adsPos;
    private Vector3 hipPos = new Vector3(0.1359997f, -0.1169999f, 0.4020013f);
    public Quaternion adsRot;
    private Quaternion hipRot;

    private bool ammoEmpty = false;
    private bool firing = false;

    private float yRecoil;
    private float recoilTimer;
    private float nextFire;

    private int currentAmmo;
    private int bulletCounter;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        ignoreRaycast = LayerMask.GetMask("IgnoreRaycast");
        hipPos = transform.parent.localPosition;
        hipRot = transform.parent.localRotation;
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        HandleADS();
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
            player.ads = false;
        }
    }

    void Shoot()
    {
        if (!ammoEmpty)
        {
            if (currentGun == CurrentGun.pistol)
            {
                FirePistol();
            }
            else if (currentGun == CurrentGun.AR)
            {
                FireAR();
            }
        }   
    }

    private void FirePistol()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firing = true;
            animator.SetTrigger("Shoot");
            ShootRayCast();
            CameraRecoil();
            currentAmmo--;
        }
        else if (currentAmmo <= 0)
        {
            ammoEmpty = true;
            firing = false;
        }
      
    }

    private void FireAR()
    {
        if (Input.GetButton("Fire1"))
        {
            firing = true;
            animator.SetBool("IsFiring", true);

            nextFire += Time.deltaTime;

            if (nextFire >= fireRate)
            {
                ShootRayCast();
                CameraRecoil();
                currentAmmo--;
                nextFire = 0;
            }

            if (currentAmmo <= 0)
            {
                animator.SetBool("IsFiring", false);
                ammoEmpty = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("IsFiring", false);
            firing = false;
        }
    }

    private void HandleADS()
    {
        if (/*transform.localPosition != adsPos &&*/ player.ads)
        {
            //transform.localPosition = Vector3.Lerp(transform.localPosition, adsPos, adsSpeed);
            transform.parent.localPosition = Vector3.Lerp(transform.parent.localPosition, adsPos, adsInSpeed);
            //transform.parent.localRotation = adsRot;
            //transform.localRotation = adsRot;
        }
        else if(/*transform.parent.localPosition != hipPos &&*/ !player.ads)
        {
            transform.parent.localPosition = Vector3.Lerp(transform.parent.localPosition, hipPos, adsOutSpeed);
            //transform.parent.localRotation = hipRot;
            //transform.localRotation = hipTransform.localRotation;
        }
    }


    void ShootRayCast()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(camera.cameraTransform.position, camera.cameraTransform.forward, out hitInfo, range, ~ignoreRaycast))
        {
            Debug.Log(hitInfo.transform.name);
            bulletHoles.bulletHoles[bulletCounter++ % (int)maxAmmo].transform.position = hitInfo.point - Camera.main.transform.forward * 0.01f /*targetDirection.normalized * 0.01f*/;
            bulletHoles.bulletHoles[bulletCounter % (int)maxAmmo].transform.rotation = Quaternion.LookRotation(hitInfo.normal);

            //Hiteffect
            GameObject impactGO = Instantiate(impactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(impactGO, 2f);
        }
    }

    void CameraRecoil()
    {
        if (firing)
        {         
            // Rotate camera up when shooting
            if (player.ads)
            {
                camera.newCameraXrotation -= adsRecoil;
            }
            else
            {
                camera.newCameraXrotation -= hipfireRecoil;

                if (currentGun == CurrentGun.AR)
                {
                    // Rotate camera sideways when shooting with AR
                    camera.camerYrecoil = Random.value - 0.5f;
                }           
            }
        }
    }

    private void OnEnable()
    {
        player.ads = false;
        transform.parent.localPosition = hipPos;
     
    }
}
