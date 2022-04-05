using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthTracker))]
public class HealthReverter : MonoBehaviour, IPlaceableObject {
    public void RevertToPlacement() {
        HealthTracker tracker = GetComponent<HealthTracker>();
        tracker.FullRestore();
    }
}
