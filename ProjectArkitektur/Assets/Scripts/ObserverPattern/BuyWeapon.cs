using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyWeapon : MonoBehaviour
{
    [SerializeField] private BoolEvent buyEvent;
    //[SerializeField] private TMPro.TMP_Text text;

    [SerializeField] private ARPickUp weaponManager;

    private void Start() => buyEvent.EventAR += UpdateboughtValue;

    

    //private void UpdateAmmo(int newAmmo) => text.text = $"Ammo: {newAmmo}";

    private void UpdateboughtValue(int addValue) => weaponManager.boughtValue += addValue;
}
