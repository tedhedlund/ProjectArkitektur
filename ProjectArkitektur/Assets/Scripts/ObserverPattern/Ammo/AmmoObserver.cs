using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AmmoObserver : MonoBehaviour
{
    [SerializeField] private EventSO ammoEvent;

    [SerializeField] private GunController arGunController;
    [SerializeField] private GunController handGunController;

    private void Start() => ammoEvent.maxAmmo += UpdateAmmo;

    private void OnDestroy() => ammoEvent.maxAmmo -= UpdateAmmo;

    private void UpdateAmmo()
    {
        arGunController.currentTotalAmmo = arGunController.ammoMaxCapacity;
        handGunController.currentTotalAmmo = handGunController.ammoMaxCapacity;
    }

}
