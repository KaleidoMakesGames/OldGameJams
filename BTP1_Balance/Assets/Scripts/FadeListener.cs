using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FadeListener : MonoBehaviour {
    public UnityEvent OnFinishedFading;
    public void FinishedFading() {
        OnFinishedFading.Invoke();
    }
}
