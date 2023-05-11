using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IntEventListener : MonoBehaviour
{
    public UnityEvent<int> OnEvent;
    [SerializeField] private IntEventSO m_event;


    private void Start() => m_event.EventAmmo += InvokeEvent;

    private void OnDestroy() => m_event.EventAmmo -= InvokeEvent;

    private void InvokeEvent(int value)
    {
        OnEvent?.Invoke(value);
    }
}
