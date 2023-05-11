using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyManager : MonoBehaviour
{
    [SerializeField] private BoolEvent valueAddedEvent;
    [SerializeField] private BoolEvent valueUpdatedEvent;
    private int boughtValue = 0;

    private void Start() => valueAddedEvent.EventAR += UpdateAmmo;

    

    private void UpdateAmmo(int addedValue)
    {
        boughtValue += addedValue;
        valueUpdatedEvent.InvokeARBuy(boughtValue);
    }
}
