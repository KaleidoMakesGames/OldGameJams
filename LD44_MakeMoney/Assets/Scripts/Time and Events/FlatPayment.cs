using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Make Money/Payments/Flat")]
public class FlatPayment : Payment {
    public float amount;
    public override float GetAmount(MoneyTracker tracker) {
        return amount;
    }

    public override string GetAmountText() {
        return amount.ToString("C2");
    }
}
