using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] private IntEventSO ammoEvent;
    //[SerializeField] private TMPro.TMP_Text text;

    [SerializeField] private GunController gunController;

    private void Start() => ammoEvent.Event += UpdateAmmo;

    private void OnDestroy() => ammoEvent.Event -= UpdateAmmo;

    //private void UpdateAmmo(int newAmmo) => text.text = $"Ammo: {newAmmo}";

    private void UpdateAmmo(int newAmmo) => gunController.currentTotalAmmo += newAmmo;
}
