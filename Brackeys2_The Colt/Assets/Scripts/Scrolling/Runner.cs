using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Runner : MonoBehaviour
{
    public Vector2 runningVector;

    public static Runner Instance;

    public void SetRunX(float x) {
        runningVector.x = x;
    }

    public void SetRunY(float y) {
        runningVector.y = y;
    }

    private void Awake() {
        if(Instance != null) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }

    private void Start() {
        RunCountTracker.Instance.numberOfRuns++;
    }
}
