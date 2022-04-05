using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToyDetector : MonoBehaviour {
    [System.Serializable]
    public class DetectionEvent : UnityEvent<ToyController> { }

    public DetectionEvent OnToyDetected;

    public bool alsoDetectWhenPickedUp;

    private void OnTriggerEnter2D(Collider2D collision) {
        ToyController tc = collision.GetComponent<ToyController>();
        if (tc != null) {
            if (!tc.isPickedUp || alsoDetectWhenPickedUp) {
                OnToyDetected.Invoke(tc);
            }
        }
    }
}
