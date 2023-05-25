using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControllerScript : MonoBehaviour
{
    //Description
    //The controller class acts as an intermediary between the model and view. For example it could listen for button clicks to call view and model.
    [SerializeField] UIModelScript uiModel;    
    [SerializeField] Player_Controller pController;
    [SerializeField] WeaponManager weaponManager;
    [SerializeField] GunController handGunController;
    [SerializeField] GunController ARGunController;

    // Update is called once per frame
    void Update()
    {
        uiModel.playerHealth = pController.health;
        uiModel.UpdateViewlHealth();

        if(weaponManager.selectedWeapon == 0)
        {
            uiModel.currentAmmoInMag = handGunController.currentAmmoInMag;
            uiModel.currentTotalAmmo = handGunController.currentTotalAmmo - handGunController.currentAmmoInMag;
            uiModel.UpdateViewAmmo();
        }
        else
        {
            uiModel.currentAmmoInMag = ARGunController.currentAmmoInMag;
            uiModel.currentTotalAmmo = ARGunController.currentTotalAmmo - ARGunController.currentAmmoInMag;
            uiModel.UpdateViewAmmo();
        }

        uiModel.UpdateViewKills();
    }
}
