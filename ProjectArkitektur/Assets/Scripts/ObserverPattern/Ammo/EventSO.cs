using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EventSO : ScriptableObject
{
    public delegate void SetMaxAmmo();
    public SetMaxAmmo maxAmmo;

    // Check if there is any subscribers, if there are we invoke
    public void InvokeAmmo() => maxAmmo?.Invoke();

}
