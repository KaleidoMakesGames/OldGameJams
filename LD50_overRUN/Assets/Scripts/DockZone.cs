using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DockZone : MonoBehaviour
{

    public static Dictionary<Boat.Location, DockZone> zoneForLocation;

    private BoxCollider2D dockZone;
    public Boat.Location location;

    private void Awake() {
        dockZone = GetComponent<BoxCollider2D>();
        if(zoneForLocation == null) {
            zoneForLocation = new Dictionary<Boat.Location, DockZone>();
        }
        zoneForLocation[location] = this;
    }

    public Vector2 GetRandomPoint() {
        var randomPointInCollider = new Vector2(Random.value, Random.value) * dockZone.size - dockZone.size / 2 + dockZone.offset;
        return transform.TransformPoint(randomPointInCollider);
    }
}
