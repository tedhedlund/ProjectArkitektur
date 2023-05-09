using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private int ammoPerMag = 30;
    [SerializeField] private int ammoMaxCapacity = 60;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 100f;
    [SerializeField] private float fireRate = 0.06f;
    [SerializeField] private float recoilAmount;
    [SerializeField] private BulletHoles bulletHoles;
    [SerializeField] private Player_Look camera;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private ParticleSystem muzzleFlash;
    public enum CurrentGun { pistol, AR };
    public CurrentGun currentGun;

    private Animator animator;
    private bool ammoInMagEmpty = false;
    private bool ammoTotalEmpty = false;
    private bool firing = false;

    private float yRecoil;
    private float recoilTimer;
    private float nextFire;

    private int currentAmmoInMag;
    public int currentTotalAmmo;
    private int bulletCounter;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentAmmoInMag = ammoPerMag;
        currentTotalAmmo = ammoMaxCapacity;
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
        Reload();
    }

    void Reload()
    {
        if (!ammoTotalEmpty)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                animator.SetTrigger("Reload");
                currentAmmoInMag = ammoPerMag;
                ammoInMagEmpty = false;
            }
        }
       
    }

    void Shoot()
    {
        Debug.Log(currentTotalAmmo);
        Debug.Log(ammoTotalEmpty);
        if (!ammoInMagEmpty && !ammoTotalEmpty)
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
            currentAmmoInMag--;
            currentTotalAmmo--;

        }
        else if (currentAmmoInMag <= 0)
        {
            ammoInMagEmpty = true;
            firing = false;
        }
        else if (currentTotalAmmo <= 0)
        {
            ammoTotalEmpty = true;
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
                currentAmmoInMag--;
                currentTotalAmmo--;
                nextFire = 0;
            }

            if (currentAmmoInMag <= 0)
            {
                animator.SetBool("IsFiring", false);
                ammoInMagEmpty = true;
            }
            else if (currentTotalAmmo <= 0)
            {
                ammoTotalEmpty = true;
                firing = false;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("IsFiring", false);
            firing = false;
        }
    }
    void ShootRayCast()
    {
        muzzleFlash.Play();

        RaycastHit hitInfo;
        if (Physics.Raycast(camera.cameraTransform.position, camera.cameraTransform.forward, out hitInfo, range))
        {
            Debug.Log(hitInfo.transform.name);
            bulletHoles.bulletHoles[bulletCounter++ % (int)ammoPerMag].transform.position = hitInfo.point - Camera.main.transform.forward * 0.01f /*targetDirection.normalized * 0.01f*/;
            bulletHoles.bulletHoles[bulletCounter % (int)ammoPerMag].transform.rotation = Quaternion.LookRotation(hitInfo.normal);

            //Hiteffect
            GameObject impactGO = Instantiate(impactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(impactGO, 2f);

          
        }
    }

    void CameraRecoil()
    {
        if (firing)
        {
            camera.newCameraXrotation -= recoilAmount;
            camera.camerYrecoil = Random.value - 0.5f;
        }
    }
}
