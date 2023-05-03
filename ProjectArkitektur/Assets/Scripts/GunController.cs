using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private int maxAmmo = 30;
    [SerializeField] private float reloadTime;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float fireRate = 0.06f;
    [SerializeField] private float hipfireRecoil;
    [SerializeField] private float adsRecoil;
    [SerializeField] private float adsInSpeed;
    [SerializeField] private float adsOutSpeed;

    [Header("Weapon animation Settings")]
    [SerializeField] private float idleAdsBob = 0.3f;
    [SerializeField] private float idleBob = 0.4f;
    [SerializeField] private float walkBob = 1.3f;    
    [SerializeField] private float sprintBobSpeed = 2;
    [SerializeField] private float reloadSpeed;

    [Header("Script Settings")]
    [SerializeField] private BulletHoles bulletHoles;
    [SerializeField] private Player_Look fpsCamera;
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
    private bool reloading = false;

    private float yRecoil;
    private float reloadTimer;
    private float nextFire;
    private float defaultAnimSpeed = 1.0f;
    private float debugBobSpeed;

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
        HandleWeaponBob();
        Reload();
        Shoot();
    }

    void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !reloading)
        {
            animator.SetTrigger("Reload");
            currentAmmo = maxAmmo;
            ammoEmpty = false;
            player.ads = false;
            reloading = true;
        }

        if (reloading)
        {
            reloadTimer += Time.deltaTime;
            if (reloadTimer >= reloadTime)
            {
                reloading = false;
                reloadTimer = 0;
            }
        }
        //Check if reload animation has finished playing
        //if (animator.GetCurrentAnimatorStateInfo(0).IsName("Reload") && 
        //    animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        //{
        //    reloading = false;
        //}
    }

    void Shoot()
    {
        if (!ammoEmpty && !reloading)
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
            animator.SetTrigger("Shoot");
            ShootRayCast();
            CameraRecoil();
            currentAmmo--;
        }
        else if (currentAmmo <= 0)
        {
            ammoEmpty = true;
        }
      
    }

    private void FireAR()
    {
        if (Input.GetButton("Fire1"))
        {
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
        }
    }

    private void HandleADS()
    {
        if (player.ads)
        {
            transform.parent.localPosition = Vector3.Lerp(transform.parent.localPosition, adsPos, adsInSpeed);
        }
        else if(!player.ads)
        {
            transform.parent.localPosition = Vector3.Lerp(transform.parent.localPosition, hipPos, adsOutSpeed);
        }
    }

    private void HandleWeaponBob()
    {

        firing = animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot");


        if (!firing && !reloading)
        {
            if (player.moveStatus == Player_Controller.MoveStatus.idle)
            {
                if (player.ads)
                {
                    animator.speed = idleAdsBob;
                }
                else animator.speed = idleBob;
            }
            else if (player.moveStatus == Player_Controller.MoveStatus.walking)
            {
                animator.speed = walkBob;
            }
            else if (player.moveStatus == Player_Controller.MoveStatus.sprinting)
            {
                animator.speed = sprintBobSpeed;
            } 
        }
        else if(firing)
        {
            animator.speed = defaultAnimSpeed;
        }
        else
        {
            animator.speed = reloadSpeed;
        }

     
        debugBobSpeed = animator.speed;
    }


    void ShootRayCast()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(fpsCamera.cameraTransform.position, fpsCamera.cameraTransform.forward, out hitInfo, range, ~ignoreRaycast))
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
                fpsCamera.newCameraXrotation -= adsRecoil;
            }
            else
            {
                fpsCamera.newCameraXrotation -= hipfireRecoil;

                if (currentGun == CurrentGun.AR)
                {
                    // Rotate camera sideways when shooting with AR
                   
                }   
                
            }
            fpsCamera.camerYrecoil = Random.value - 0.5f;
        }
    }


    private void OnEnable()
    {
        player.ads = false;
        transform.parent.localPosition = hipPos;   
    }
}
