using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoneyTracker : MonoBehaviour
{
    public UnityEvent OnWentBroke;

    public UnityEvent OnFirstMoneyGain;

    private bool hasGained;

    public float currentMoney;

    private void Awake() {
        hasGained = false;
    }

    public void GainMoney(float amount) {
        SetMoney(currentMoney + amount);
        if(!hasGained) {
            OnFirstMoneyGain.Invoke();
            hasGained = true;
        }
    }

    public void SpendMoney(float amount) {
        SetMoney(currentMoney - amount);
    }

    public void SetMoney(float amount) {
        currentMoney = amount;
        if(currentMoney < 0) {
            OnWentBroke.Invoke();
        }
    }
}
