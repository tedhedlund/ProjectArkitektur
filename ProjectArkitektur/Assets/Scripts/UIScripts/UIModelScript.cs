using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIModelScript : MonoBehaviour
{
    //Description
    //The model class contains the data and logic for the UI. For example it stores the current score and calls methods for updating it.

    //Health of player
    //Ammo of player
    //Kills

    [SerializeField] UIViewScript uiView;    
    
    public int playerHealth;
    
    public int currentAmmoInMag;
    public int currentTotalAmmo;

    public int curKills = 0;

    public void UpdateViewlHealth()
    {
        uiView.UpdateHealth(playerHealth);
    }

    public void UpdateViewAmmo()
    {
        uiView.UpdateAmmoMag(currentAmmoInMag);
        uiView.UpdateAmmoTotal(currentTotalAmmo);
    }

    public void UpdateViewKills()
    {
        uiView.UpdateKills(curKills);
    }
    
}