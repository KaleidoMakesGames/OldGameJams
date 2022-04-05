using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoleDetector : MonoBehaviour
{
    public UnityEvent OnFill;

    public bool isFilled {
        get {
            return colliders.Count > 0;
        }
    }

    private List<Collider> colliders;

    private void Awake() {
        colliders = new List<Collider>();
    }

    private void OnTriggerEnter(Collider other) {
        colliders.Add(other);
        OnFill.Invoke();
    }

    private void OnTriggerExit(Collider other) {
        if (colliders.Contains(other)) {
            colliders.Remove(other);
        }
    }
}
