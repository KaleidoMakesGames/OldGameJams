using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerDistanceTracker : MonoBehaviour
{
    public float distanceTraveled { get; private set; }

    private void Awake() {
        distanceTraveled = 0.0f;
    }

    private void Update() {
        if (Runner.Instance.enabled) {
            distanceTraveled += Runner.Instance.runningVector.magnitude * Time.deltaTime;
        }
    }
}
