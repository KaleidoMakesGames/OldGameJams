using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetirementCalculator : MonoBehaviour
{
    public MoneyTracker moneyTracker;
    public TimeTracker timeTracker;

    public float amountNeededPerYear;
    public float estimatedDeathAge;

    public float amountNeededToRetireNow {
        get {
            return (estimatedDeathAge - timeTracker.currentAge) * amountNeededPerYear;
        }
    }

    public bool canRetire {
        get {
            return moneyTracker.currentMoney >= amountNeededToRetireNow;
        }
    }
}
