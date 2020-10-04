using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StrikesCounter : MonoBehaviour
{
    [ReadOnly] public int strikes;
    public int untilLoss;
    public UnityEvent OnStrike;
    public UnityEvent OnStrikeOut;

    public void RegisterStrikes(int count) {
        strikes = Mathf.Clamp(strikes+count, 0, untilLoss+1);
        OnStrike.Invoke();
        if(strikes >= untilLoss) {
            OnStrikeOut.Invoke();
        }
    }
}
