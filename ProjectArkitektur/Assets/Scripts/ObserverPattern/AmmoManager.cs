using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    [SerializeField] private IntEventSO ammoAddedEvent;
    [SerializeField] private IntEventSO ammoUpdatedEvent;
    private int currentAmmo = 0; 

    private void Start() => ammoAddedEvent.Event += UpdateAmmo;

    private void OnDestroy() => ammoAddedEvent.Event -= UpdateAmmo; //Os�ker p� vad detta g�r f�r v�r ammologik

    private void UpdateAmmo(int addedAmmo)
    {
        currentAmmo += addedAmmo;
        ammoUpdatedEvent.Invoke(currentAmmo);
    }
}
