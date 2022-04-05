using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Navigation;

[RequireComponent(typeof(NavigationTargeter))]
public class WaypointChainFollower : MonoBehaviour {
    public WaypointChainHolder chainToFollow;

    public Transform currentWaypoint;

    private NavigationTargeter targeter;

    private int nextWaypointIndex;

    private void Awake() {
        targeter = GetComponent<NavigationTargeter>();
    }

    private void Start() {
        ResetFollower();
    }

    private void Update() {
        if(currentWaypoint != null && Vector2.Distance(targeter.transform.position, currentWaypoint.position) <= targeter.goalStoppingDistance) {
            AdvanceToNextWaypoint();
        }
    }

    private void AdvanceToNextWaypoint() {
        if (chainToFollow != null && nextWaypointIndex < chainToFollow.waypoints.Count) {
            currentWaypoint = chainToFollow.waypoints[nextWaypointIndex];
            nextWaypointIndex++;
        } else {
            currentWaypoint = null;
        }
    }

    public void ResetFollower() {
        nextWaypointIndex = 0;
        AdvanceToNextWaypoint();
    }
}
