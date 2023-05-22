using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] private IntEventSO ammoEvent;
    //[SerializeField] private TMPro.TMP_Text text;

    [SerializeField] private GunController gunController;
    [SerializeField] private GunController handGunController;

    //private void Start() => ammoEvent.EventAmmo += UpdateAmmo;

    // Test
    private void Start() => ammoEvent.maxAmmo += UpdateAmmo;

    //private void OnDestroy() => ammoEvent.EventAmmo -= UpdateAmmo;

    //private void UpdateAmmo(int newAmmo) => text.text = $"Ammo: {newAmmo}";

    private void UpdateAmmo(/*int newAmmo*/)  //=> gunController.currentTotalAmmo += newAmmo;
    {
        //gunController.currentTotalAmmo += newAmmo;
        //handGunController.currentTotalAmmo += newAmmo;

        // Added for test without argument
        gunController.currentTotalAmmo = gunController.ammoMaxCapacity;
        handGunController.currentTotalAmmo = handGunController.ammoMaxCapacity;
    }

}
