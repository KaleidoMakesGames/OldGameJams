using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public float timePerRound;
    public float timeRemaining;

    public UnityEvent OnTimeout;

    private void Update() {
        timeRemaining = Mathf.Clamp(timeRemaining - Time.deltaTime, 0.0f, Mathf.Infinity);
        if(timeRemaining == 0.0f) {
            OnTimeout.Invoke();
        }
    }
}
