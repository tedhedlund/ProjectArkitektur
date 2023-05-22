using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IntEventListener : MonoBehaviour
{
    public UnityEvent<int> OnEvent;
    public delegate void MaxAmmo();
    MaxAmmo maxAmmoDelegate;
    
    [SerializeField] private IntEventSO m_event;


    //private void Start() => m_event.EventAmmo += InvokeEvent;

    //private void OnDestroy() => m_event.EventAmmo -= InvokeEvent;


    //// Funktionen blir observer till delegaten
    //private void Start() => m_event.maxAmmo += InvokeEvent; // Adds itself to the subscriber list

    //private void OnDestroy() => m_event.maxAmmo -= InvokeEvent; // Removes itself to the subscriber list


    //// Check if there is any subscribers, if there are we invoke
    //private void InvokeEvent(/*int value*/)
    //{
    //    maxAmmoDelegate?.Invoke(/*value*/);
    //}
}
