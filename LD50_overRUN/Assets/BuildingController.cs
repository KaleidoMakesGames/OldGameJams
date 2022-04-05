using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class BuildingController : MonoBehaviour
{
    public SoldierController unitOnRoof;
    public Transform roofSpot;
    public Transform roofDoor;
    public Transform groundDoor;
    public Transform groundSpot;

    private void Awake() {
        if(unitOnRoof != null) {
            unitOnRoof.currentBuilding = this;
        }
    }

    private void Update() {
        if(!Application.isPlaying) {
            if(unitOnRoof != null) {
                unitOnRoof.transform.position = roofSpot.position;
            }
        }
    }
}
