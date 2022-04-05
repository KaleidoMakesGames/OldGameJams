using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenTracker : MonoBehaviour {
    public static List<OxygenTracker> allTrackers;

    private float _currentOxygen;
    public float currentOxygen {
        get {
            return _currentOxygen;
        }
        set {
            _currentOxygen = Mathf.Clamp(value, 0.0f, maxOxygen);
        }
    }

    public float initialOxygen = 5;
    public int maxOxygen = 5;

    // How much oxygen is generated per second by this naturally
    public float generationRate;
    
    // Oxygen flows from low priority to high priority (not same level)
    public int oxygenPriority;

    // Oxygen trackers within this distance of higher priority are given oxygen over time.
    public float providingRadius;
    // Amount of oxygen diffused to each high priority tracker in range per second.
    public float providingRate;

    public GridElement gridElement { get; private set; }

    private void Awake() {
        gridElement = GetComponent<GridElement>();
        currentOxygen = initialOxygen;
        if(allTrackers == null) {
            allTrackers = new List<OxygenTracker>();
        }
        allTrackers.Add(this);
    }

    private void OnDestroy() {
        allTrackers.Remove(this);
    }

    private void Update() {
        currentOxygen += generationRate * Time.deltaTime;

        if(providingRadius == 0 || providingRate == 0.0f) {
            return;
        }

        List<OxygenTracker> trackers = new List<OxygenTracker>();
        for(int x = -(int)providingRadius; x <= (int)providingRadius; x++) {
            for(int y = -(int)providingRadius; y <= (int)providingRadius; y++) {
                Vector2Int point = new Vector2Int(x, y);
                if(point.magnitude <= providingRadius) {
                    foreach(GridElement e in WorldGrid.Instance.ElementsAtPosition(point+gridElement.position)) {
                        OxygenTracker tracker = e.GetComponent<OxygenTracker>();
                        if(tracker != null && tracker != this && tracker.oxygenPriority > oxygenPriority) {
                            trackers.Add(tracker);
                        }
                    }
                }
            }
        }
        
        foreach(OxygenTracker tracker in trackers) {
            float toGive = Mathf.Min(providingRate * Time.deltaTime, currentOxygen);

            float room = tracker.maxOxygen - tracker.currentOxygen;
            float given = Mathf.Min(toGive, room);

            tracker.currentOxygen += given;
            currentOxygen -= given;
        }
    }
}
