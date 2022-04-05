using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WaypointChainFollower))]
public class WaypointChainFinder : MonoBehaviour {
    private void Awake() {
        GetComponent<WaypointChainFollower>().chainToFollow = FindObjectOfType<WaypointChainHolder>(); 
    }
}
