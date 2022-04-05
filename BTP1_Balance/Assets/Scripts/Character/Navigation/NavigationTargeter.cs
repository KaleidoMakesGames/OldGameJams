using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Navigation;

[RequireComponent(typeof(NavigatableAreaPathfinder))]
public class NavigationTargeter : MonoBehaviour {
    public Transform self;
    public Transform target;
    public float goalStoppingDistance;

    public NavigationTargeterSettings settings;

    public Queue<Vector2> waypoints { get; private set; }

    public bool isAtTarget {
        get {
            return !isRecomputing && waypoints == null;
        }
    }

    private bool isRecomputing;

    private Vector3 lastTargetPosition;
    private float lastUpdateTime;

    private void Awake() {
        isRecomputing = false;
    }

    private void Update() {
        UpdateWaypoints();
    }

    private void UpdateWaypoints() {
        if (target == null) {
            waypoints = null;
            return;
        }

        if(ShouldRecomputeWaypoints()) {
            RecomputeWaypoints();
            lastUpdateTime = Time.time;
            lastTargetPosition = target.position;
        }
        
        if (waypoints != null) {
            if(waypoints.Count == 0) {
                waypoints.Enqueue(target.position);
            }

            while (waypoints.Count > 0) {
                if (Vector2.Distance(self.position, target.position) <= goalStoppingDistance) {
                    waypoints = null;
                    break;
                }
                if (Vector2.Distance(waypoints.Peek(), self.position) <= settings.reachedWaypointDistance) {
                    waypoints.Dequeue();
                } else {
                    break;
                }
            }
        }
    }

    private bool ShouldRecomputeWaypoints() {
        float timeSinceLastUpdate = Time.time - lastUpdateTime;

        if (timeSinceLastUpdate < 1 / settings.maxUpdateFrequency) {
            return false;
        }

        if (timeSinceLastUpdate >= 1/settings.minUpdateFrequency) {
            return true;
        }

        if (lastTargetPosition != target.position) {
            return true;
        }

        return false;
    }

    public void RecomputeWaypoints() {
        if (target != null) {
            if(Vector2.Distance(self.position, target.position) <= goalStoppingDistance) {
                waypoints = null;
                return;
            }

            isRecomputing = true;
            StartCoroutine(GetComponent<NavigatableAreaPathfinder>().FindPathToTarget(target.position, delegate (List<Vector2> waypointList) {
                isRecomputing = false;
                if (waypointList == null) {
                    waypoints = null;
                    return;
                }
                waypoints = new Queue<Vector2>(waypointList);
            }));
        } else {
            waypoints = null;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        if(waypoints != null) {
            Vector2? lastPoint = null;
            foreach(Vector2 point in waypoints) {
                if(lastPoint.HasValue) {
                    Gizmos.DrawLine(lastPoint.Value, point);
                }
                lastPoint = point;
            }
        }
    }
}
