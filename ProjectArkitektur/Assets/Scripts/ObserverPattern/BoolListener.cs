using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class BoolListener : MonoBehaviour
{
    public UnityEvent<int> OnEvent;
    [SerializeField] private BoolEvent int_event;


    private void Start() => int_event.EventAR += InvokeEvent;

    

    private void InvokeEvent(int value)
    {
        OnEvent?.Invoke(value);
    }
}
