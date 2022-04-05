using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
    public static Dictionary<Boat.Location, Island> islandForLocation;
    public Boat.Location location;
    
    public BoxCollider2D c { get; private set; }

    public static Island AtPoint(Vector2 p) {
        foreach(var island in islandForLocation.Values) {
            if(island.c.OverlapPoint(p)) {
                return island;
            }
        }
        return null;
    }

    public List<SoldierController> UnitsOnIsland() {
        var units = new List<Collider2D>();
        c.OverlapCollider(new ContactFilter2D(), units);
        return units.Select(x => x.GetComponentInParent<SoldierController>()).Where(x => x != null).ToList();
    }

    private void Awake() {
        if(islandForLocation == null) {
            islandForLocation = new Dictionary<Boat.Location, Island>();
        }
        islandForLocation[location] = this;
        c = GetComponent<BoxCollider2D>();
    }
}
