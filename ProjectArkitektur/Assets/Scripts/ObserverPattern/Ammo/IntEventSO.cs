using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class IntEventSO : ScriptableObject
{
    public event Action<int> EventAmmo;

    public delegate void SetMaxAmmo();
    public SetMaxAmmo maxAmmo;
    

    public void InvokeAmmo(int value) => EventAmmo?.Invoke(value);

    // Check if there is any subscribers, if there are we invoke
    public void InvokeAmmo() => maxAmmo?.Invoke();

}
