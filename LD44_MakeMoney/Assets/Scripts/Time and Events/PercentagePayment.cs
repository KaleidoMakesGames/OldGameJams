using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Make Money/Payments/Percentage")]
public class PercentagePayment : Payment
{
    public float percentage;

    public override float GetAmount(MoneyTracker tracker) {
        return tracker.currentMoney * (percentage/100.0f);    
    }

    public override string GetAmountText() {
        return (percentage/100.0f).ToString("P2") + " of savings";
    }
}
