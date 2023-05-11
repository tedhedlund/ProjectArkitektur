using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    [SerializeField] private IntEventSO ammoAddedEvent;
    [SerializeField] private IntEventSO ammoUpdatedEvent;
    private int currentAmmo = 0; 

    private void Start() => ammoAddedEvent.EventAmmo += UpdateAmmo;

    private void OnDestroy() => ammoAddedEvent.EventAmmo -= UpdateAmmo; //Os�ker p� vad detta g�r f�r v�r ammologik

    private void UpdateAmmo(int addedAmmo)
    {
        currentAmmo += addedAmmo;
        ammoUpdatedEvent.InvokeAmmo(currentAmmo);
    }
}
