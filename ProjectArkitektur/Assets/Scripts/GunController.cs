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
    [SerializeField] private float recoilAmount;
    [SerializeField] private BulletHoles bulletHoles;
    //[SerializeField] private Camera fpsCamera;
    [SerializeField] private Player_Look camera;
    public enum CurrentGun { pistol, AR };
    public CurrentGun currentGun;

    private Animator animator;
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
    void ShootRayCast()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(camera.cameraTransform.position, camera.cameraTransform.forward, out hitInfo, range))
        {
            Debug.Log(hitInfo.transform.name);
            bulletHoles.bulletHoles[bulletCounter++ % (int)maxAmmo].transform.position = hitInfo.point - Camera.main.transform.forward * 0.01f /*targetDirection.normalized * 0.01f*/;
            bulletHoles.bulletHoles[bulletCounter % (int)maxAmmo].transform.rotation = Quaternion.LookRotation(hitInfo.normal);
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
