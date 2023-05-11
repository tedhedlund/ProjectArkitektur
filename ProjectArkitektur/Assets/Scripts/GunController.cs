using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private int ammoPerMag;
    [SerializeField] private int ammoMaxCapacity;
    [SerializeField] private float reloadTime;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float fireRate = 0.06f;
    [SerializeField] private float hipfireRecoil;
    [SerializeField] private float adsRecoil;
    [SerializeField] private float adsInSpeed;
    [SerializeField] private float adsOutSpeed;
    [SerializeField] private float goTosprintPosSpeed;

    [Header("Weapon animation Settings")]
    [SerializeField] private float idleAdsBob = 0.3f;
    [SerializeField] private float idleBob = 0.4f;
    [SerializeField] private float walkBob = 1.3f;    
    [SerializeField] private float sprintBobSpeed = 2;
    [SerializeField] private float reloadSpeed;
    [SerializeField] private ParticleSystem muzzleFlash;

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

    public Vector3 hipPos; //= new Vector3(0.1359997f, -0.09f, 0.4020013f);
    public Vector3 sprintPos;// = new Vector3(0.02f, -0.08f, 0.31f);
    public Quaternion sprintRot; //= Quaternion.Euler(12.88f, 354.68f, 25.5f);
    public Quaternion adsRot; //= Quaternion.Euler(359.6f, 0f, 0f);
    private Quaternion hipRot;

    private bool ammoEmpty = false;
    private bool firing = false;
    private bool reloading = false;

    private float yRecoil;
    private float reloadTimer;
    private float nextFire;
    private float defaultAnimSpeed = 1.0f;
    private float debugBobSpeed;

    private int currentAmmoInMag;
    private int currentTotalAmmo;
    private int bulletCounter;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        ignoreRaycast = LayerMask.GetMask("IgnoreRaycast");
        hipPos = transform.parent.localPosition;
        hipRot = transform.parent.localRotation;
        currentAmmoInMag = ammoPerMag;
        currentTotalAmmo = ammoMaxCapacity;
     
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
        if (Input.GetKeyDown(KeyCode.R) && !reloading && !firing && currentTotalAmmo > 0)
        {
            animator.SetTrigger("Reload");
            ammoEmpty = false;
            player.ads = false;
            reloading = true;

            if (currentTotalAmmo < ammoPerMag)
            {
                currentAmmoInMag = currentTotalAmmo;
            }
            else
            {
                currentAmmoInMag = ammoPerMag;
            }
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
        Debug.Log($"Total ammo: {currentTotalAmmo}");
        Debug.Log($"Total ammo: {currentAmmoInMag}");

        if (currentAmmoInMag > 0)
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
            currentAmmoInMag--;
            currentTotalAmmo--;
        }
        else if (currentAmmoInMag <= 0)
        {
            ammoEmpty = true;
        }
      
    }

    private void FireAR()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            animator.SetBool("IsFiring", true);

            nextFire += Time.deltaTime;

            if (nextFire >= fireRate)
            {
                ShootRayCast();
                CameraRecoil();
                currentAmmoInMag--;
                currentTotalAmmo--;
                nextFire = 0;
            }

            if (currentAmmoInMag <= 0 || currentTotalAmmo <= 0)
            {
                animator.SetBool("IsFiring", false);
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
            transform.parent.localRotation = Quaternion.Lerp(transform.parent.localRotation,adsRot, goTosprintPosSpeed);
        }
        else if(!player.ads && player.moveStatus != Player_Controller.MoveStatus.sprinting)
        {
            transform.parent.localPosition = Vector3.Lerp(transform.parent.localPosition, hipPos, adsOutSpeed);
            transform.parent.localRotation = Quaternion.Lerp(transform.parent.localRotation, hipRot, goTosprintPosSpeed);
        }
        else if(player.moveStatus == Player_Controller.MoveStatus.sprinting && player.moveStatus != Player_Controller.MoveStatus.idle)
        {
            transform.parent.localPosition = Vector3.Lerp(transform.parent.localPosition, sprintPos, goTosprintPosSpeed);
            transform.parent.localRotation = Quaternion.Lerp(transform.parent.localRotation, sprintRot, goTosprintPosSpeed);
        }
        //else if(!player.ads && player.moveStatus == Player_Controller.MoveStatus.strafing)
        //{

        //}
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
        muzzleFlash.Play();

        RaycastHit hitInfo;
        Vector3 startPos = fpsCamera.cameraTransform.position;
        if (Physics.Raycast(startPos, fpsCamera.cameraTransform.forward, out hitInfo, range, ~ignoreRaycast))
        {
            // Add extra hipfire recoil if player is not ADS
            if (!player.ads)
            {
                Vector3 newHitPos = new Vector3(hitInfo.point.x - Random.Range(-(hipfireRecoil * 1.5f), hipfireRecoil * 1.5f), hitInfo.point.y + Random.Range(0.0f, 0.7f), hitInfo.point.z);
                Vector3 newFireDir = newHitPos - startPos;
                if (Physics.Raycast(startPos, newFireDir, out hitInfo, range, ~ignoreRaycast))
                {
                    HandleBulletHit(hitInfo);
                }
            }
            else
            {
                HandleBulletHit(hitInfo);
            }
        }
    }

    void HandleBulletHit(RaycastHit hitInfo)
    {
        Debug.Log(hitInfo.transform.name);
        bulletHoles.bulletHoles[bulletCounter++ % (int)ammoPerMag].transform.position = hitInfo.point - Camera.main.transform.forward * 0.01f /*targetDirection.normalized * 0.01f*/;
        bulletHoles.bulletHoles[bulletCounter % (int)ammoPerMag].transform.rotation = Quaternion.LookRotation(hitInfo.normal);

        //Hiteffect
        GameObject impactGO = Instantiate(impactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
        Destroy(impactGO, 2f);
    }


    void CameraRecoil()
    {
        //if (firing)
        //{         
        // Rotate camera up when shooting
        if (player.ads)
        {
            fpsCamera.newCameraXrotation -= adsRecoil;

            if (currentGun == CurrentGun.AR)
            {
                // Rotate camera sideways when shooting with AR
                //fpsCamera.camerYrecoil = Random.value - 0.5f;
                fpsCamera.camerYrecoil = Random.Range(-0.3f, 0.3f);
            }
        }
        else
        {
            fpsCamera.newCameraXrotation -= hipfireRecoil;

            if (currentGun == CurrentGun.AR)
            {
                // Rotate camera sideways when shooting with AR
                //fpsCamera.camerYrecoil = Random.value - 0.5f;
                fpsCamera.camerYrecoil = Random.Range(-0.5f, 0.5f);
            }   
                
        }
        //}
    }


    private void OnEnable()
    {
        player.ads = false;
        transform.parent.localPosition = hipPos;   
    }
}
