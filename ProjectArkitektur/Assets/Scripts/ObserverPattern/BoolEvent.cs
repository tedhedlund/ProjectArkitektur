using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class BoolEvent : ScriptableObject
{
    
    public event Action<int> EventAR;

    
    public void InvokeARBuy(int bought) => EventAR?.Invoke(bought);
}
