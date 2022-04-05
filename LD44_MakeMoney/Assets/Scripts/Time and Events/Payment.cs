using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Payment : ScriptableObject
{
    public bool isCharge;

    public abstract float GetAmount(MoneyTracker tracker);
    public abstract string GetAmountText();
}
