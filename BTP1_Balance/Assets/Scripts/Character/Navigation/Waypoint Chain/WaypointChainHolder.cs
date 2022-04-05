using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class WaypointChainHolder : MonoBehaviour {
    public List<Transform> waypoints {
        get {
            List<Transform> newWaypoints = new List<Transform>();

            foreach(Transform t in transform) {
                newWaypoints.Add(t);
            }

            return newWaypoints;
        }
    }

    private void OnDrawGizmosSelected() {
        Transform lastTransform = null;
        int number = 1;
        foreach(Transform t in transform) {
            Gizmos.DrawSphere(t.position, 0.1f);
            if(lastTransform != null) {
                Gizmos.DrawLine(lastTransform.position, t.position);
            }
            lastTransform = t;

#if UNITY_EDITOR
            Handles.Label(t.position, "Waypoint " + number.ToString());
#endif
            number++;
        }
    }
}
