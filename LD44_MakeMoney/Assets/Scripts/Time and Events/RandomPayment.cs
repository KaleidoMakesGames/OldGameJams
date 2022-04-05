using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Make Money/Payments/Random")]
public class RandomPayment : Payment
{
    public float low;
    public float high;
    public override float GetAmount(MoneyTracker tracker) {
        return Random.Range(low, high);
    }

    public override string GetAmountText() {
        return low.ToString("C2") + "-" + high.ToString("C2");
    }
}
