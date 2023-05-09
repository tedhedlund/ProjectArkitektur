using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    [SerializeField] private IntEventSO ammoAddedEvent;
    [SerializeField] private IntEventSO ammoUpdatedEvent;
    private int currentAmmo = 0; 

    private void Start() => ammoAddedEvent.Event += UpdateAmmo;

    private void OnDestroy() => ammoAddedEvent.Event -= UpdateAmmo; //Osäker på vad detta gör för vår ammologik

    private void UpdateAmmo(int addedAmmo)
    {
        currentAmmo += addedAmmo;
        ammoUpdatedEvent.Invoke(currentAmmo);
    }
}
