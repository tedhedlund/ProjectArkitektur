using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class IntEventSO : ScriptableObject
{
    public event Action<int> EventAmmo;
    

    public void InvokeAmmo(int value) => EventAmmo?.Invoke(value);
    
}
